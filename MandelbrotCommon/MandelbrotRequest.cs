using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotCommon
{
    public class MandelbrotRequest
    {
        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public double Left
        {
            get;
            set;
        }

        public double Right
        {
            get;
            set;
        }

        public double Top
        {
            get;
            set;
        }

        public double Bottom
        {
            get;
            set;
        }

        public uint Iterations
        {
            get;
            set;
        }

    }
}
