using MandelbrotCommon;
using MandelbrotMenge.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Calculator
{
    public class StandardMandelbrotCalculator : IMandelbrotCalculator
    {
        public Task<uint[,]> CalculateAsync(MandelbrotRequest req)
        {
            uint[,] MBMap = new uint[req.Width, req.Height];

            double deltaX = req.Left - req.Right;
            deltaX = deltaX < 0 ? deltaX * (-1) : deltaX;
            double stepSizeX = deltaX / req.Width;

            double deltaY = req.Bottom - req.Top;
            deltaY = deltaY < 0 ? deltaY * (-1) : deltaY;
            double stepSizeY = deltaY / req.Height;

            for (int x = 0; x < req.Width; x ++)
            {
                double xpos = req.Left + x * stepSizeX;
                for (int y = 0; y < req.Height; y ++)
                {
                    double ypos = req.Bottom + y * stepSizeY;

                    ComplexNumber c = new ComplexNumber(xpos, ypos);
                    ComplexNumber z = new ComplexNumber(0, 0);

                    uint iteration = 0;
                    do
                    {
                        iteration++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2.0)
                            break;
                    }
                    while (iteration < req.Iterations);

                    MBMap[x, y] = iteration;
                }
            }

            return Task.FromResult(MBMap);
        }
    }
}
