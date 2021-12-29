using MandelbrotServer.EventArgs;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    public partial class RequestHandler
    {
        private uint[,] calculatedMandelbrot;

        private SinkAdapter adapter;

        private object locker;

        private int reqHeight;
        private int reqWidth;

        private uint lastID;

        private void ComposeProcessedBlocks(object sender, ProcessedBlockEventArgs e)
        {
            try
            {
                if (e.Topic != this.handlerID)
                    return;

                int blockwidth;
                if (e.ID == this.lastID)
                {
                    blockwidth = this.reqWidth % this.blockthickness;
                }
                else
                {
                    blockwidth = this.blockthickness;
                }

                var data = TransformResponse(e.Data, blockwidth, reqHeight);

                var offsetLeft = (e.ID - 1) * blockthickness;

                this.FillBlockIntoMandelbrot(offsetLeft, data);

                lock (locker1)
                {
                    this.blockCounter -= 1;
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }

        public uint[,] TransformResponse(byte[] input, int width, int height)
        {
            uint[,] data = new uint[width, height];

            try
            {
                
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int index = (x * height + y) * 4;
                        data[x, y] = BinaryPrimitives.ReadUInt32LittleEndian(input.AsSpan().Slice(index, 4));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        private void FillBlockIntoMandelbrot(long offsetLeft, uint[,] data)
        {
            var width = data.GetLength(0);
            var height = data.GetLength(1);

            lock (locker)
            {
                for (int y = 0; y < height; y++)
                {
                    for (long x = offsetLeft; x < width + offsetLeft; x++)
                    {
                        try
                        {
                            this.calculatedMandelbrot[x, y] = data[x - offsetLeft, y];
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}
