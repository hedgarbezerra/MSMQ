using MSMQ.Common.Messages;
using MSMQ.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Factories
{
    public interface IRabbitConsumerFactory
    {
        List<IRabbitConsumer> Create(CommonMessage message);
    }

    public class RabbitConsumerFactory : IRabbitConsumerFactory
    {
        private readonly IRabbitQueuesGenerator _queuesGenerator;

        private Dictionary<string, List<IRabbitConsumer>> _queueConsumers;

        public RabbitConsumerFactory(IRabbitQueuesGenerator queuesGenerator, IEnumerable<IRabbitConsumer> consumers)
        {
            _queuesGenerator = queuesGenerator;

            _queueConsumers = consumers.GroupBy(k => k.Queue)
                .ToDictionary(k => k.Key, g => g.ToList());
        }

        public List<IRabbitConsumer> Create(CommonMessage message)
        {
            var queue = _queuesGenerator.GetQueue(message);

            if (!_queueConsumers.TryGetValue(queue, out var consumers))
                throw new InvalidOperationException("Consumer for selected qeueue not implemented.");

            return consumers;
        }
    }
}
