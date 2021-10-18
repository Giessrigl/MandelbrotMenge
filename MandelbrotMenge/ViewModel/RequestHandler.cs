using MandelbrotCommon;
using MandelbrotMenge.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace MandelbrotMenge.ViewModel
{
    public class HttpRequestHandler : IRequestHandler
    {
        public async Task<byte[]> PostAsync(string url, MandelbrotRequest req)
        {
            Url Path = new Url(url);
            var response = await Path.AppendPathSegments("api", "mandelbrot").PostJsonAsync(req);

            return await response.GetBytesAsync();
        }
    }
}
