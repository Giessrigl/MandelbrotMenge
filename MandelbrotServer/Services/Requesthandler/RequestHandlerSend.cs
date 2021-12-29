using MandelbrotCommon;
using MandelbrotServer.EventArgs;
using MandelbrotServer.Services.Interfaces;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    // See also RequesthandlerReceive for the complement
    public partial class RequestHandler
    {
        private IVentilator vent;

        private string handlerID;

        private int blockthickness;

        private int blockCounter;

        private object locker1 = new object();

        public RequestHandler(IVentilator vent, SinkAdapter adapter)
        {
            this.vent = vent;
            this.adapter = adapter;

            this.handlerID = Guid.NewGuid().ToString();
            this.locker = new object();
            this.blockthickness = 100;
        }

        public async Task<uint[,]> ProcessRequest(MandelbrotRequest req)
        {
            this.reqHeight = req.Height;
            this.reqWidth = req.Width;

            this.adapter.OnFinishedBlock += this.ComposeProcessedBlocks;
            this.calculatedMandelbrot = new uint[req.Width, req.Height];

            this.blockCounter = this.DistributeBlocks(req);

            while (! (this.blockCounter <= 0))
            {
                await Task.Delay(100);
            }

            return await Task.FromResult(this.calculatedMandelbrot);
        }

        private int DistributeBlocks(MandelbrotRequest req)
        {
            var tempArea = new CoordinateSystemArea(req.Left, req.Right, req.Bottom, req.Top);
            var tempWidth = req.Width;

            uint id = 0;

            // split into rows
            while (tempWidth > 0)
            {
                id++;
                var blockThickness = Math.Min(tempWidth, this.blockthickness);
                var calculatedBlock = this.CalculateBlock(tempArea, tempWidth, blockThickness);

                MandelbrotWorkerRequest message = new MandelbrotWorkerRequest()
                {
                    Bottom = calculatedBlock.Limit_YAxis_Bottom,
                    Top = calculatedBlock.Limit_YAxis_Top,
                    Left = calculatedBlock.Limit_XAxis_Left,
                    Right = calculatedBlock.Limit_XAxis_Right,
                    Height = reqHeight,
                    Width = blockThickness,
                    Iterations = req.Iterations,
                    Topic = this.handlerID,
                    Id = id,
                };

                var data = JsonSerializer.Serialize<MandelbrotWorkerRequest>(message);
                this.vent.PushToQueue(data);

                tempWidth -= blockThickness;
            }

            this.lastID = id;
            return (int)id;
        }

        private CoordinateSystemArea CalculateBlock(CoordinateSystemArea area, int width, int blockThickness)
        {
            var currentBlockLeft = area.Limit_XAxis_Left;
            double widthDelta = (blockThickness + 0.0) / width;

            var absImageRangeX = this.CalculateDistance(area.Limit_XAxis_Left, area.Limit_XAxis_Right);

            area.Limit_XAxis_Left += widthDelta * absImageRangeX;

            var result = new CoordinateSystemArea(currentBlockLeft, area.Limit_XAxis_Left, area.Limit_YAxis_Bottom, area.Limit_YAxis_Top);

            return result;
        }

        // Calculates the absolute difference between two numbers.
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
