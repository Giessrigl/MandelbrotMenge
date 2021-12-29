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
            var message = e.Message;

            var topic = message.Pop().ConvertToString();
            if (topic != this.handlerID)
                return;

            var id = BinaryPrimitives.ReadUInt32LittleEndian(message.Pop().Buffer);
            var response = message.Pop().Buffer;

            int blockwidth;
            if (id == this.lastID)
            {
                blockwidth = this.reqWidth % this.blockthickness;
            }
            else
            {
                blockwidth = this.blockthickness;
            }

            var data = TransformResponse(response, blockwidth);

            var offsetLeft = (id - 1) * blockthickness;

            this.FillBlockIntoMandelbrot(offsetLeft - 1, data, blockwidth);
        }

        private uint[,] TransformResponse(byte[] res, int blockwidth)
        {
            uint[,] result = new uint[blockwidth, this.reqHeight];

            for (int y = 0; y < this.reqHeight; y++)
            {
                for (int x = 0; x < blockwidth; x++)
                {
                    var value = BinaryPrimitives.ReadUInt32LittleEndian(res);

                    result[x, y] = value;
                }
            }

            return result;
        }

        private void FillBlockIntoMandelbrot(long offsetLeft, uint[,] data, int blockwidth)
        {
            lock (locker)
            {
                for (int y = 0; y < reqHeight; y++)
                {
                    for (long x = offsetLeft; x < blockwidth; x++)
                    {
                        this.calculatedMandelbrot[x, y] = data[x - offsetLeft, y];
                    }
                }
            }
        }
    }
}
