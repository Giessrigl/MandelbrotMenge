using MandelbrotServer.EventArgs;
using MandelbrotServer.Services.Interfaces;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    public class Sink : ISink
    {
        public event EventHandler<ProcessedBlockEventArgs> OnFinishedBlock;

        public Sink()
        {
            this.Start();
        }

        private void Start()
        {
            while (true)
            {
                using (var receiver = new PullSocket("@tcp://localhost:5558"))
                {
                    receiver.Poll();
                    var message = receiver.ReceiveMultipartMessage(3);

                    var topic = message.Pop().ConvertToString();
                    var id = BinaryPrimitives.ReadUInt32LittleEndian(message.Pop().Buffer);
                    var data = message.Pop().Buffer;

                    this.FireOnFinishedBlock(new ProcessedBlockEventArgs()
                    {
                        ID = id,
                        Topic = topic,
                        Data = data
                    });
                }
            }
        }

        private void FireOnFinishedBlock(ProcessedBlockEventArgs args)
        {
            this.OnFinishedBlock?.Invoke(this, args);
        }
    }
}
