using Serilog;
using System.Diagnostics;

namespace MSMQ.Bus
{
    public class BusWorker : BackgroundService
    {
        private readonly ILogger<BusWorker> _logger;

        public BusWorker(ILogger<BusWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ActivitySource tracingSource = new("Example.Source");
            int counter = 1;
            _logger.LogInformation("Starting services...");

            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                using (var tracing = tracingSource.StartActivity($"Iterator"))
                {
                    tracing?.AddTag($"Iteração [{counter}]", counter);
                    _logger.LogInformation($"Running program for {counter}º time.");
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _logger.LogInformation($"Finished program for {counter}º time.");

                    counter++;
                }
            }
        }
    }
}
