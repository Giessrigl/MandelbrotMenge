using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotWorker
{
    public class Pusher
    {
        private readonly string ip;
        private readonly string port;

        public Pusher(string ip, string sinkPort)
        {
            this.ip = ip;
            this.port = sinkPort;
        }

        public void Push(string topic, string id, byte[] data)
        {
            using (var sender = new PushSocket(">tcp://" + this.ip + ":" + this.port))
            {
                sender.SendMoreFrame(topic).SendMoreFrame(id).SendFrame(data);
            }
        }
    }
}
