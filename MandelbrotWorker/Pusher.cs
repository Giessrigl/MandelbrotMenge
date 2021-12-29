using NetMQ;
using NetMQ.Sockets;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotWorker
{
    public class Pusher : IDisposable
    {
        private PushSocket sender;
        private object locker = new object();

        public Pusher(string ip, string sinkPort)
        {
            this.sender = new PushSocket(">tcp://" + ip + ":" + sinkPort);
        }

        public void Dispose()
        {
            this.sender.Dispose();
        }

        public void Push(string topic, uint id, byte[] data)
        {
            var IDbytes = new byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(IDbytes, id);

            lock(locker)
            {
                this.sender.SendMoreFrame(topic).SendMoreFrame(IDbytes).SendFrame(data);
            }
        }
    }
}
