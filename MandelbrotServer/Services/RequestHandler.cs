using MandelbrotCommon;
using MandelbrotServer.EventArgs;
using MandelbrotServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    public class RequestHandler
    {
        private readonly IVentilator vent;

        private readonly ISink sink;

        private string handlerID;

        private int blockthickness;

        private int reqHeight;
        private int reqWidth;

        private int blocksInWidth;
        private int blocksInHeight;

        private uint id;

        private uint[,] calculatedMandelbrot;

        private object locker;

        public RequestHandler(IVentilator vent, ISink sink)
        {
            this.locker = new object();
            this.vent = vent;
            this.sink = sink;

            this.handlerID = Guid.NewGuid().ToString();

            this.blockthickness = 100;
        }

        public uint[,] ProcessRequest(MandelbrotRequest req)
        {
            this.sink.OnFinishedBlock += ComposeProcessedBlocks;
            this.calculatedMandelbrot = new uint[req.Width, req.Height];

            this.reqHeight = req.Height;
            this.reqWidth = req.Width;

            this.blocksInWidth = int.Parse(Math.Ceiling(((req.Width + 0.0) / blockthickness)).ToString());
            this.blocksInHeight = int.Parse(Math.Ceiling(((req.Height + 0.0) / blockthickness)).ToString());

            this.DistributeBlocks(req);

            while (! this.CheckMandelbrotReady())
            {
                Thread.Sleep(3000);
            }

            return this.calculatedMandelbrot;
        }

        private bool CheckMandelbrotReady()
        {
            for (int i = 0; i < reqHeight; i++)
            {
                for (int j = 0; j < reqWidth; j++)
                {
                    if (this.calculatedMandelbrot[j, i] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void ComposeProcessedBlocks(object sender, ProcessedBlockEventArgs e)
        {
            if (e.Topic == this.handlerID)
            {
                var id = e.ID;
                var data = e.Data;

                var offsets = this.CalculateOffset(id);
                this.FillBlockIntoMandelbrot(offsets.Item1, offsets.Item2, data);
            }
        }

        private (int, int) CalculateOffset(uint id)
        {
            var blockOffsetTop = int.Parse( Math.Ceiling(id / (this.blocksInWidth + 0.0)).ToString() );
            var blockOffsetLeft = int.Parse((id % this.blocksInWidth).ToString());

            blockOffsetTop
        }

        private (int, int) CalculateProcessedBlockThickness()
        {

        }

        private void FillBlockIntoMandelbrot(int offsetLeft, int offsetTop, byte[] data)
        {
            lock(locker)
            {

            }
        }

        private void DistributeBlocks(MandelbrotRequest req)
        {
            // the limits of one pixel
            var pixelHeight = this.CalculateDistance(req.Bottom, req.Top) / req.Height;
            var pixelWidth = this.CalculateDistance(req.Left, req.Right) / req.Width;

            // the amount of pixels in the imageheight
            var blockheight = Math.Min(req.Height, this.blockthickness);
            // the amount of pixels in the imagewidth
            var blockwidth = Math.Min(req.Width, this.blockthickness);

            // the amount of blocks in the imageheigth
            var blockHeigthAmount = Math.Ceiling((req.Height + (0.0)) / blockheight);
            // the amount of blocks in the imagewidth
            var blockWidthAmount = Math.Ceiling((req.Width + (0.0)) / blockwidth);

            this.id = 0;
            var tempHeight = req.Height;
            var tempWidth = req.Width;
            for (int y = 0; y < blockHeigthAmount; y++)
            {
                this.id++;

                blockheight = Math.Min(tempHeight, this.blockthickness);
                tempHeight -= blockheight;

                for (int x = 0; x < blockWidthAmount; x++)
                {
                    blockwidth = Math.Min(tempWidth, this.blockthickness);
                    tempWidth -= blockwidth;

                    var pixelOffsetleft = this.CalculateBlockLimit(pixelWidth, blockwidth, x);
                    var pixelOffsetRight = this.CalculateBlockLimit(pixelWidth, blockwidth, x + 1);

                    var pixelOffsetBottom = this.CalculateBlockLimit(pixelHeight, blockheight, y);
                    var pixelOffsetTop = this.CalculateBlockLimit(pixelHeight, blockheight, y + 1);

                    MandelbrotRequest message = new MandelbrotRequest()
                    {
                        Bottom = pixelOffsetBottom,
                        Top = pixelOffsetTop,
                        Left = pixelOffsetleft,
                        Right = pixelOffsetRight,
                        Height = blockheight,
                        Width = blockwidth,
                        Iterations = req.Iterations,
                    };

                    var data = JsonSerializer.Serialize(message);
                    this.vent.PushToQueue(this.handlerID, id, data);
                }
            }
        }

        private double CalculateBlockLimit(double pixelThickness, int blockThickness, int blockOffsetNumber)
        {
            return (pixelThickness * blockThickness) + (blockOffsetNumber * blockThickness * pixelThickness);
        }

        // Calculates the dicstance between two values.
        private double CalculateDistance(double x, double y)
        {
            if (Math.Sign(x) == 1 && Math.Sign(y) == -1 ||
                Math.Sign(x) == -1 && Math.Sign(y) == 1)
            {
                return Math.Abs(x) + Math.Abs(y);
            }
            else
            {
                var min = Math.Min(Math.Abs(x), Math.Abs(y));
                var max = Math.Max(Math.Abs(x), Math.Abs(y));
                return max - min;
            }
        }
    }
}
