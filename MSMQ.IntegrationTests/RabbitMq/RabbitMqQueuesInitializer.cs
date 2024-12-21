using Microsoft.Extensions.DependencyInjection;
using MSMQ.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.IntegrationTests.RabbitMq
{
    internal static class RabbitMqQueuesInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider, out List<string> queues)
        {
            var rabbitQueuesGenerator = serviceProvider.GetRequiredService<IRabbitQueuesGenerator>();
            var rabbitQueuesCreator = serviceProvider.GetRequiredService<IRabbitMessagesQueueCreator>();

            queues = rabbitQueuesGenerator.GetQueues();

            rabbitQueuesCreator.CreateQueues(queues.ToArray());
        }
    }
}
