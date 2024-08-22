using MSMQ.Common.Serializers;
using MSMQ.RabbitMQ.Factories;
using MSMQ.RabbitMQ.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MSMQ.RabbitMQ
{
    public class Worker(IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var runningService = scope.ServiceProvider.GetRequiredService<IRabbitMqExecutingService>();
                await runningService.ExecuteAsync(stoppingToken);
            }
        }
    }
}
