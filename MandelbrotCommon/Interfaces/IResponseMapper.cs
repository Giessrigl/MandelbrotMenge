using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotCommon.Interfaces
{
    public interface IResponseMapper<TInput, TOutput>
    {
        TOutput Map(TInput input);
    }
}
