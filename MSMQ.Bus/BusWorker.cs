using Serilog;
using System.Diagnostics;

namespace MSMQ.Bus
{
    public class BusWorker(IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var runningService = scope.ServiceProvider.GetRequiredService<IServiceBusRunningService>();
                await runningService.Run(stoppingToken);
            }
        }
    }
}
