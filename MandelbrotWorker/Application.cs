using MandelbrotCommon;
using MandelbrotWorker.Calculator;
using NetMQ;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            this.pusher = new Pusher(ip, pull);
            this.puller = new Puller(ip, push);
            this.calc = new StandardMandelbrotCalculator();
        }

        public void Start()
        {
            while(true)
            {
                var message = this.puller.Pull();
                var msgParts = message.ToArray();
                var data = msgParts[2].ConvertToString();
                
                Console.WriteLine("Received message:");
                Console.WriteLine($"Topic: {msgParts[0]} // Part: {msgParts[1]} // Json: {msgParts[2]}");
                Console.WriteLine("");
                
                var req = JsonSerializer.Deserialize<MandelbrotRequest>(data);
                var tempRes = this.calc.CalculateAsync(req).GetAwaiter().GetResult();

                var res = this.TransformResponse(tempRes);

                NetMQMessage calculatedBlock = new NetMQMessage(new List<byte[]>{
                                                                            msgParts[0].Buffer,
                                                                            msgParts[1].Buffer,
                                                                            res
                                                                            });

                this.pusher.Push(calculatedBlock);
            }
        }

        private byte[] TransformResponse(uint[,] data)
        {
            var width = data.GetLength(0);
            var heigth = data.GetLength(1);

            Span<byte> result = new Span<byte>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < heigth; j++)
                {
                    BinaryPrimitives.WriteUInt32LittleEndian(result, data[i,j]);
                }
            }

            return result.ToArray();
        }
    }
}
