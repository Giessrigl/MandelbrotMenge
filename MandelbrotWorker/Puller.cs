using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotWorker
{
    public class Puller : IDisposable
    {
        private PullSocket receiver;

        public Puller(string ip, string ventPort)
        {
            this.receiver = new PullSocket(">tcp://" + ip + ":" + ventPort);
        }

        public void Dispose()
        {
            this.receiver.Dispose();
        }

        public NetMQMessage Pull()
        {
            return receiver.ReceiveMultipartMessage(3);
        }
    }
}
