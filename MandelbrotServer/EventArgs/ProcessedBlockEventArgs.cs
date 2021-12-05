using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.EventArgs
{
    public class ProcessedBlockEventArgs
    {
       public string Topic
        {
            get;
            set;
        }

        public uint ID
        {
            get;
            set;
        }

        public byte[] Data
        {
            get;
            set;
        }
    }
}
