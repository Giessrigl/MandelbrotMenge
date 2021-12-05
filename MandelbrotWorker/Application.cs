using MandelbrotWorker.Calculator;
using NetMQ;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotWorker
{
    public class Application
    {
        private readonly Pusher pusher;
        private readonly Puller puller;

        private readonly IMandelbrotCalculator calc;

        public Application(string ip, string push, string pull)
        {
            this.pusher = new Pusher(ip, push);
            this.puller = new Puller(ip, pull);
            this.calc = new StandardMandelbrotCalculator();
        }

        public void Start()
        {
            while(true)
            {
                var message = this.puller.Pull();
                var msgParts = message.ToArray();

                var data = msgParts[2].ConvertToString();


                NetMQMessage newMessage = new NetMQMessage(new List<byte[]>{
                                                                            msgParts[0].Buffer, 
                                                                            msgParts[1].Buffer,  
                                                                            processedWork.ToArray()
                                                                            });

                this.pusher.Push(newMessage);
            }
        }
    }
}
