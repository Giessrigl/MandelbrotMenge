using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.ResponseWrapper
{
    public interface IResponseWrapper<TInput, TOutput>
    {
        public TOutput Wrap(TInput input);
    }
}
