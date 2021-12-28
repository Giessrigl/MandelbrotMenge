using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotWorker
{
    public class Puller
    {
        private readonly string ip;
        private readonly string port;

        public Puller(string ip, string port)
        {
            this.ip = ip;
            this.port = port;
        }

        public NetMQMessage Pull()
        {
            using (var receiver = new PullSocket(">tcp://" + this.ip + ":" + this.port))
            {
                return receiver.ReceiveMultipartMessage(3);
            }
        }
    }
}
