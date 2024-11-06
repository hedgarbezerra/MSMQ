using MSMQ.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus.Factories
{
    public interface IMessageHandlersFactory
    {
        List<IMessageHandler> Create(string queueName);
    }

    public class MessageHandlersFactory : IMessageHandlersFactory
    {
        private readonly IDictionary<string, List<IMessageHandler>> _handlers;

        public MessageHandlersFactory(IEnumerable<IMessageHandler> handlers)
        {

            _handlers = handlers.GroupBy(h => h.Queue)
                .ToDictionary(h => h.Key, h => h.ToList());
        }

        public List<IMessageHandler> Create(string queueName)
        {
            if (!_handlers.TryGetValue(queueName, out List<IMessageHandler> handlers))
                throw new InvalidOperationException($"Message handler not implemented for queue '{queueName}'");

            return handlers;
        }
    }
}
