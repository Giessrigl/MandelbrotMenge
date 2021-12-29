using MandelbrotServer.Services.Interfaces;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    public class Ventilator : IVentilator, IDisposable
    {
        private object locker;
        private PushSocket sender;

        public Ventilator()
        {
            this.locker = new object();
            this.sender = new PushSocket("@tcp://*:5557");
        }

        public void Dispose()
        {
            this.sender.Dispose();
        }

        // sends the data to process together with a topic and id of the portion to the Queue ready for the workers to process it.
        public void PushToQueue(string data)
        {
            lock (locker)
            {
                this.sender.SendFrame(data);
            }
        }
    }
}
