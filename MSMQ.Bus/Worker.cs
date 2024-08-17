namespace MSMQ.Bus
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int counter = 1;
            _logger.LogInformation("Starting services...");
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(50));
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation($"Running program for {counter}º time.");
                counter++;
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                _logger.LogInformation($"Finished program for {counter}º time.");
            }
        }
    }
}
