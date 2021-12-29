using MandelbrotCommon;
using MandelbrotWorker.Calculator;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            try
            {
                while (true)
                {
                    var message = this.puller.Pull();
                    var msgParts = message.ToArray();

                    var req = JsonSerializer.Deserialize<MandelbrotWorkerRequest>(msgParts[0].ConvertToString());

                    Console.WriteLine("Received message:");
                    Console.WriteLine($"Topic: {req.Topic} // Part: {req.Id}");
                    Console.WriteLine("");

                    MandelbrotRequest req1 = new MandelbrotRequest()
                    {
                        Top = req.Top,
                        Bottom = req.Bottom,
                        Left = req.Left,
                        Right = req.Right,
                        Iterations = req.Iterations,
                        Height = req.Height,
                        Width = req.Width,
                    };

                    var tempRes = this.calc.CalculateAsync(req1).GetAwaiter().GetResult();

                    var res = this.TransformResponse(tempRes);

                    this.pusher.Push(req.Topic, req.Id, res);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private byte[] TransformResponse(uint[,] data)
        {
            var width = data.GetLength(0);
            var height = data.GetLength(1);

            Span<byte> result = new Span<byte>(new byte[4]);
            List<byte> output = new List<byte>(data.Length * 4);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!BinaryPrimitives.TryWriteUInt32LittleEndian(result, data[x,y]))
                    {
                        throw new ArgumentOutOfRangeException(nameof(result), "Could not write all bytes into the byte array.");
                    }
                    output.AddRange(result.ToArray());
                }
            }

            return output.ToArray();
        }
    }
}
