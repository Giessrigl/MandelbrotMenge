using MandelbrotCommon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotMenge.Interfaces
{
    public interface IRequestHandler
    {
        public Task<byte[]> PostAsync(string url, MandelbrotRequest req);
    }
}
