using MandelbrotServer.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Services.Interfaces
{
    public interface ISink
    {
        public SinkAdapter adapter
        {
            get;
        }
    }
}
