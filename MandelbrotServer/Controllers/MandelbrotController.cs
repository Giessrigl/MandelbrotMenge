using MandelbrotCommon;
using MandelbrotCommon.Interfaces;
using MandelbrotServer.Calculator;
using MandelbrotServer.ResponseWrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MandelbrotServer.Controllers
{
    [ApiController]
    [Route("/api/mandelbrot")]
    public class MandelbrotController : ControllerBase
    {
        private readonly IMandelbrotCalculator mbc;
        private readonly IResponseMapper<uint[], byte[]> mapper;

        public MandelbrotController(IMandelbrotCalculator mbc, 
                                    IResponseMapper<uint[], byte[]> wrapper)
        {
            this.mbc = mbc;
            this.mapper = wrapper;
        }

        [HttpPost]
        public async Task CalculateMandelbrot([FromBody] MandelbrotRequest req)
        {
            var result = await this.mbc.CalculateAsync(req);
            var reshapedResult = await this.ReshapeForStream(result);
            var response = this.mapper.Map(reshapedResult);

            Response.ContentType = "application/octet-stream";
            await Response.Body.WriteAsync(response, 0, response.Length);
            await Response.StartAsync();

            await Response.CompleteAsync();
        }

        private Task<uint[]> ReshapeForStream(uint[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            uint[] result = new uint[width * height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[(i * width) + j] = array[j, i];
                }
            }

            return Task.FromResult(result);
        }
    }
}
