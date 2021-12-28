using MandelbrotServer.Services.Interfaces;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    public class Ventilator : IVentilator, IDisposable
    {
        private PushSocket sender;

        private object locker;

        public Ventilator()
        {
            this.sender = new PushSocket("@tcp://*:5557");
            this.locker = new object();
        }

        public void Dispose()
        {
            sender.Dispose();
        }

        // sends the data to process together with a topic and id of the portion to the Queue ready for the workers to process it.
        public void PushToQueue(string topic, uint id, string data)
        {
            lock(locker)
            {
                this.sender.SendMoreFrame(topic).SendMoreFrame(id.ToString()).SendFrame(data);
            }
        }
    }
}
