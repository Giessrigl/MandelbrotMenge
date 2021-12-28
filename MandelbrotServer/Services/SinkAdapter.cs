using MandelbrotServer.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services
{
    public class SinkAdapter
    {
        public event EventHandler<ProcessedBlockEventArgs> OnFinishedBlock;

        internal void FireOnFinishedBlock(ProcessedBlockEventArgs args)
        {
            this.OnFinishedBlock?.Invoke(this, args);
        }
    }
}
