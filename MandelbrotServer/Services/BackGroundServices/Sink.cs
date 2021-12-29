using MandelbrotServer.EventArgs;
using MandelbrotServer.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using NetMQ;
using NetMQ.Sockets;
using System.Buffers.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotServer.Services.BackgroundServices
{
    public class Sink : BackgroundService, ISink
    {
        private SinkAdapter adapter;
        private PullSocket receiver;

        public Sink(SinkAdapter adapter)
        {
            this.adapter = adapter;
            this.receiver = new PullSocket("@tcp://localhost:5558");
        }

        SinkAdapter ISink.adapter => this.adapter;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                await Task.Yield();
                var message = this.receiver.ReceiveMultipartMessage(3);

                var topic = message.Pop().ConvertToString();
                var id = BinaryPrimitives.ReadUInt32LittleEndian(message.Pop().Buffer);
                var data = message.Pop().Buffer;

                this.adapter.FireOnFinishedBlock(new ProcessedBlockEventArgs()
                {
                    Topic = topic,
                    ID = id,
                    Data = data,
                });
            }
        }
    }
}
