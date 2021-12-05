using MandelbrotServer.Services.Interfaces;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    // id, data, result
    public class Ventilator : IVentilator
    {
        private readonly ISink sink;

        public Ventilator(ISink sink)
        {
            this.sink = sink;
        }

        // sends the data to process together with a topic and id of the portion to the Queue ready for the workers to process it.
        public void PushToQueue(string topic, uint id, string data)
        {
            using (var sender = new PushSocket("@tcp://*:5557"))
            {
                sender.SendMoreFrame(topic);
                sender.SendMoreFrame(id.ToString());
                sender.SendFrame(data);
            }
        }
    }
}
