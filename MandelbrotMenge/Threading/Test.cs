using MandelbrotCommon;
using MandelbrotCommon.Interfaces;
using MandelbrotMenge.Interfaces;
using MandelbrotMenge.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotMenge.Threading
{
    public class Test
    {
        private IRequestHandler handler;
        private IResponseMapper<byte[], uint[,]> mapper;
        private BitmapPainter bmPainter;

        private int blockPixelThickness;

        public Test(IRequestHandler handler, IResponseMapper<byte[], uint[,]> mapper)
        {
            this.handler = handler;
            this.mapper = mapper;
            this.blockPixelThickness = 3;
        }

        // Splits the Image into small pieces and distributes them into threads.
        public void TestMethod(ImageVM image, CoordinateSystemAreaVM area, uint iterations)
        {
            this.bmPainter = new BitmapPainter(image.Width, image.Height);

            var tempArea = new CoordinateSystemAreaVM(area.Limit_XAxis_Left, area.Limit_XAxis_Right, area.Limit_YAxis_Bottom, area.Limit_YAxis_Top);
            var tempHeight = image.Height;

            int offsetX = 0; // ignore
            int offsetY = 0;

            // split into rows
            while(tempHeight > 0)
            {
                var blockThickness = Math.Min(tempHeight, this.blockPixelThickness);
                var calculatedBlock = this.CalculateBlock(tempArea, tempHeight, blockThickness);

                var thread = new Thread(this.Worker);
                thread.Start(new ThreadArguments(
                                                 image, 
                                                 image.Width, 
                                                 blockThickness,
                                                 offsetX, 
                                                 offsetY, 
                                                 calculatedBlock,
                                                 iterations));

                tempHeight -= blockThickness;
                offsetY = image.Height - tempHeight;
            }
        }

        // Every Worker handles a portion of the Mandelbrot request.
        private async void Worker(object data)
        {
            if (! (data is ThreadArguments))
            {
                throw new ArgumentException(nameof(data), $"data must be of type {nameof(ThreadArguments)}.");
            }

            var threadArgs = (ThreadArguments)data;

            var response = await this.handler.PostMandelbrotAsync("https://localhost:44329",
                                new MandelbrotRequest()
                                {
                                    Width = threadArgs.Blockwidth,
                                    Height = threadArgs.Blockheight,
                                    Left = threadArgs.Area.Limit_XAxis_Left,
                                    Right = threadArgs.Area.Limit_XAxis_Right,
                                    Top = threadArgs.Area.Limit_YAxis_Top,
                                    Bottom = threadArgs.Area.Limit_YAxis_Bottom,
                                    Iterations = threadArgs.Iterations
                                });

            var map = this.mapper.Map(response, threadArgs.Blockwidth, threadArgs.Blockheight);
            threadArgs.Image.BmpImage = this.bmPainter.PaintBitmap(threadArgs.StartX, threadArgs.StartY, map);
        }

        // Calculates the new limits of a portion of the mandelbrot.
        private CoordinateSystemAreaVM CalculateBlock(CoordinateSystemAreaVM area, int height, int blockThickness)
        {
            var currentBlockBottom = area.Limit_YAxis_Bottom;
            double heightDelta = (blockThickness + 0.0) / height;

            var absImageRangeY = this.CalculateDistance(area.Limit_YAxis_Bottom, area.Limit_YAxis_Top);

            area.Limit_YAxis_Bottom += heightDelta * absImageRangeY;

            var result = new CoordinateSystemAreaVM(area.Limit_XAxis_Left, area.Limit_XAxis_Right, currentBlockBottom, area.Limit_YAxis_Bottom);

            return result;
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
                var max = Math.Max (Math.Abs(x), Math.Abs(y));
                return max - min;
            }
        }
    }
}
