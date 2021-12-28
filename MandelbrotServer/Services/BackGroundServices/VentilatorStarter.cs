using MandelbrotServer.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotServer.Services.BackgroundServices
{
    public class VentilatorStarter : IHostedService
    {
        public VentilatorStarter(IVentilator ventilator)
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Yield();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}