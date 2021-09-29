using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotMenge.MbC
{
    public interface IMandelbrotCalculator
    {
        public int Steps_XAxis
        {
            get;
            set;
        }

        public int Steps_YAxis
        {
            get;
            set;
        }

        public int[,] Calculate(CoordinateSystemArea csArea);
    }
}
