using MSMQ.Common;
using MSMQ.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Services
{
    public interface IRabbitMessagesQueueCreator
    {
        void CreateQueues(string exchange, params string[] queues);
        void CreateQueues(params string[] queues);
    }

    public class RabbitMessagesQueueCreator(IRabbitQueueCreator _queueCreator, IRabbitExchangeCreator _exchangeCreator, ILogger<RabbitMessagesQueueCreator> _logger) : IRabbitMessagesQueueCreator
    {
        public void CreateQueues(string exchange, params string[] queues)
        {
            try
            {
                _exchangeCreator.Create(exchange);

                _logger.LogInformation("Creating {QueuesCount} new queues to '{ExchangeName}', if necessary.", queues.Length, exchange);

                foreach (var q in queues) 
                { 
                    _queueCreator.Create(q, exchange);

                    _logger.LogInformation("'{QueueName}' created to exchange '{ExchangeName}' successfully.", q, exchange);
                }

                _logger.LogInformation("Queues were created successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating queues, reason: {ErrorReason}", e.Message);
            }
        }

        public void CreateQueues(params string[] queues)
        {
            try
            {
                _logger.LogInformation("Creating {QueuesCount} new queues to undefined exchange, if necessary.", queues.Length);

                foreach (var q in queues)
                {
                    _queueCreator.Create(q);

                    _logger.LogInformation("'{QueueName}' created to undefined exchange successfully.", q);
                }

                _logger.LogInformation("Queues were created successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating queues, reason: {ErrorReason}", e.Message);
            }
        }
    }
}
