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
    public class Ventilator : IVentilator
    {
        private object locker;
        private PushSocket sender;

        public Ventilator()
        {
            this.locker = new object();
            this.sender = new PushSocket("@tcp://*:5557");
        }

        // sends the data to process together with a topic and id of the portion to the Queue ready for the workers to process it.
        public void PushToQueue(string topic, string id, string data)
        {
            lock (locker)
            {
                this.sender.SendMoreFrame(topic).SendMoreFrame(id).SendFrame(data);
            }
        }
    }
}
