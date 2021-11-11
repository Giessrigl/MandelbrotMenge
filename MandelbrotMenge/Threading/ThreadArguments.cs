using MandelbrotCommon.Interfaces;
using MandelbrotMenge.Interfaces;
using MandelbrotMenge.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotMenge.Threading
{
    public class ThreadArguments
    {
        public int StartX
        {
            get;
            private set;
        }

        public int StartY
        {
            get;
            private set;
        }

        public int Blockwidth
        {
            get;
            private set;
        }

        public int Blockheight
        {
            get;
            private set;
        }

        public CoordinateSystemAreaVM Area
        {
            get;
            private set;
        }

        public ImageVM Image
        {
            get;
            private set;
        }

        public uint Iterations
        {
            get;
            private set;
        }

        public ThreadArguments(
                                ImageVM image, 
                                int width, 
                                int height, 
                                int startX, 
                                int startY, 
                                CoordinateSystemAreaVM area, 
                                uint iterations)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.Image = image;
            this.Blockwidth = width;
            this.Blockheight = height;
            this.Area = area;
            this.Iterations = iterations;
        }
    }
}
