using MandelbrotCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.Calculator
{
    public interface IMandelbrotCalculator
    {
        public Task<uint[,]> CalculateAsync(MandelbrotRequest req);
    }
}
