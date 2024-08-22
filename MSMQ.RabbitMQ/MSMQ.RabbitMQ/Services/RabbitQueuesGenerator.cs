using MSMQ.Common.Messages;
using MSMQ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Services
{
    public interface IRabbitQueuesGenerator
    {
        string GetQueue(CommonMessage message);
        string GetQueue(Type payloadType);
        List<string> GetQueues();
    }

    public class RabbitQueuesGenerator : IRabbitQueuesGenerator
    {
        public List<string> GetQueues()
        {
            var expectedType = typeof(IRabbitConsumer);

            var implementations = expectedType
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && expectedType.IsAssignableFrom(t))
                .ToList();

            //Must have base class and a single generic parameter
            var impGenericParameters = implementations.Select(i => i.BaseType.GenericTypeArguments.First());

            var queues = impGenericParameters
                .Select(GetQueue)
                .Distinct()
                .ToList();

            return queues;
        }

        public string GetQueue(CommonMessage message)
        {
            var payloadType = message.GetPayloadType();

            return GetQueue(payloadType);
        }

        public string GetQueue(Type payloadType) => $"queue-{payloadType?.FullName?.ToLower().Trim()}";
    }
}
