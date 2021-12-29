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
            await Task.Yield();
            while (true)
            {
                var message = this.receiver.ReceiveMultipartMessage(3);

                this.adapter.FireOnFinishedBlock(new ProcessedBlockEventArgs()
                {
                    Message = message
                });
            }
        }
    }
}
