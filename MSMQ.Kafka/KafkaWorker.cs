using MSMQ.Kafka.Services;

namespace MSMQ.Kafka
{
    public class KafkaWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public KafkaWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var runningService = scope.ServiceProvider.GetRequiredService<IKafkaRunningService>();
                await runningService.Run(stoppingToken);
            }
        }
    }
}
