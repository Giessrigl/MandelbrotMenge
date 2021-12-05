using MandelbrotCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotWorker.Calculator
{
    public interface IMandelbrotCalculator
    {
        Task<uint[,]> CalculateAsync(MandelbrotRequest req);
    }
}
