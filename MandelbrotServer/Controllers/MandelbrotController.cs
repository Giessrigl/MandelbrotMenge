using MandelbrotCommon;
using MandelbrotCommon.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MandelbrotServer.Services.Interfaces;
using MandelbrotServer.Services;

namespace MandelbrotServer.Controllers
{
    [ApiController]
    [Route("/api/mandelbrot")]
    public class MandelbrotController : ControllerBase
    {
        private readonly IResponseMapper<uint[], byte[]> mapper;
        private readonly RequestHandler reqHandler;

        public MandelbrotController(IResponseMapper<uint[], byte[]> wrapper, RequestHandler reqHandler)
        {
            this.mapper = wrapper;
            this.reqHandler = reqHandler;
        }

        [HttpPost]
        public async Task CalculateMandelbrot([FromBody] MandelbrotRequest req)
        {
            var result = this.reqHandler.ProcessRequest(req);

            var reshapedResult = await this.ReshapeForStream(result);
            var response = this.mapper.Map(reshapedResult, 0, 0);

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
