using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotMenge.MbC
{
    public class CoordinateSystemArea
    {
        public double limit_XAxis_Left
        {
            get;
            private set;
        }

        public double limit_XAxis_Right
        {
            get;
            private set;
        }

        public double limit_YAxis_Bottom
        {
            get;
            private set;
        }

        public double limit_YAxis_Top
        {
            get;
            private set;
        }

        public CoordinateSystemArea(double x1, double x2, double y1, double y2)
        {
            this.limit_XAxis_Left = x1;
            this.limit_XAxis_Right = x2;
            this.limit_YAxis_Bottom = y1;
            this.limit_YAxis_Top = y2;
        }
    }
}
