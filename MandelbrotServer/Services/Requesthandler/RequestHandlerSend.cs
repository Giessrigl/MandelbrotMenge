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

        public RequestHandler(IVentilator vent, SinkAdapter adapter)
        {
            this.vent = vent;
            this.adapter = adapter;

            this.handlerID = Guid.NewGuid().ToString();
            this.locker = new object();
            this.blockthickness = 500;
        }

        public async Task<uint[,]> ProcessRequest(MandelbrotRequest req)
        {
            this.reqHeight = req.Height;
            this.reqWidth = req.Width;

            this.adapter.OnFinishedBlock += this.ComposeProcessedBlocks;
            this.calculatedMandelbrot = new uint[req.Width, req.Height];

            this.DistributeBlocks(req);

            while (!this.CheckMandelbrotReady())
            {
                Thread.Sleep(3000);
            }

            this.adapter.OnFinishedBlock -= ComposeProcessedBlocks;

            return await Task.FromResult(this.calculatedMandelbrot);
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

        private void DistributeBlocks(MandelbrotRequest req)
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

                MandelbrotRequest message = new MandelbrotRequest()
                {
                    Bottom = calculatedBlock.Limit_YAxis_Bottom,
                    Top = calculatedBlock.Limit_YAxis_Top,
                    Left = calculatedBlock.Limit_XAxis_Left,
                    Right = calculatedBlock.Limit_XAxis_Right,
                    Height = reqHeight,
                    Width = tempWidth,
                    Iterations = req.Iterations,
                };

                var data = JsonSerializer.Serialize(message);
                this.vent.PushToQueue(this.handlerID, id.ToString(), data);

                tempWidth -= blockThickness;
            }

            this.lastID = id;
        }

        private CoordinateSystemArea CalculateBlock(CoordinateSystemArea area, int width, int blockThickness)
        {
            var currentBlockLeft = area.Limit_XAxis_Left;
            double widthDelta = (blockThickness + 0.0) / width;

            var absImageRangeY = this.CalculateDistance(area.Limit_YAxis_Bottom, area.Limit_YAxis_Top);

            area.Limit_XAxis_Left += widthDelta * absImageRangeY;

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
