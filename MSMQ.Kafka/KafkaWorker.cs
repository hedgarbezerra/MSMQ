using Confluent.Kafka;
using MSMQ.Kafka.Factories;
using MSMQ.Kafka.Handlers;
using MSMQ.Kafka.Messages;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
