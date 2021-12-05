using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotCommon
{
    public class CoordinateSystemArea
    {
        public double Limit_XAxis_Left
        {
            get;
            set;
        }

        public double Limit_XAxis_Right
        {
            get;
            set;
        }

        public double Limit_YAxis_Bottom
        {
            get;
            set;
        }

        public double Limit_YAxis_Top
        {
            get;
            set;
        }

        public CoordinateSystemArea(double x1, double x2, double y1, double y2)
        {
            this.Limit_XAxis_Left = x1;
            this.Limit_XAxis_Right = x2;
            this.Limit_YAxis_Bottom = y1;
            this.Limit_YAxis_Top = y2;
        }
    }
}
