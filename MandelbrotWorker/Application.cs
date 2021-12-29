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

        public Application(string ip, string vent, string sink)
        {
            this.pusher = new Pusher(ip, sink);
            this.puller = new Puller(ip, vent);
            this.calc = new StandardMandelbrotCalculator();
        }

        public void Start()
        {
            while(true)
            {
                var message = this.puller.Pull();
                var msgParts = message.ToArray();

                var topic = msgParts[0].ConvertToString();
                var id = msgParts[1].ConvertToString();
                var data = msgParts[2].ConvertToString();
                
                Console.WriteLine("Received message:");
                Console.WriteLine($"Topic: {topic} // Part: {id}");
                Console.WriteLine("");
                
                var req = JsonSerializer.Deserialize<MandelbrotRequest>(data);
                var tempRes = this.calc.CalculateAsync(req).GetAwaiter().GetResult();

                var res = this.TransformResponse(tempRes);

                this.pusher.Push(topic, id, res);
            }
        }

        private byte[] TransformResponse(uint[,] data)
        {
            var width = data.GetLength(0);
            var heigth = data.GetLength(1);

            Span<byte> result = new Span<byte>(new byte[4]);
            List<byte> output = new List<byte>(width * heigth * 4);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < heigth; j++)
                {
                    if (!BinaryPrimitives.TryWriteUInt32LittleEndian(result, data[i,j]))
                    {
                        throw new ArgumentOutOfRangeException(nameof(result), "Could not write all bytes into the byte array.");
                    }
                }
                output.AddRange(result.ToArray());
            }

            return output.ToArray();
        }
    }
}
