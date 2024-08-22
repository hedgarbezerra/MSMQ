using MSMQ.Common.Messages;
using MSMQ.RabbitMQ.Factories;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Services
{
    public interface IRabbitConsumer
    {
        string Queue { get; }
        Task Consume(CommonMessage message);
    }

    public abstract class RabbitConsumer<T>(ILogger<RabbitConsumer<T>> _logger, IRabbitQueuesGenerator _qGenerator) : IRabbitConsumer where T : class
    {
        public string Queue => _qGenerator.GetQueue(typeof(T));

        public async Task Consume(CommonMessage message)
        {
            string queueName = _qGenerator.GetQueue(message);
            _logger.LogInformation("Started consuming message #{MessageId} from queue '{QueueName}' by consumer '{ConsumerName}'.", message.Id, queueName, this.GetType().Name);

            var payload = (T)message.Payload;
            await Handle(payload);

            _logger.LogInformation("Message #{MessageId} from queue '{QueueName}' has been consumed by consumer '{ConsumerName}'.", message.Id, queueName, this.GetType().Name);
        }

        protected abstract Task Handle(T payload);
    }
}
