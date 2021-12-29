using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.EventArgs
{
    public class ProcessedBlockEventArgs
    {
        public NetMQMessage Message
        {
            get;
            set;
        }
    }
}
