using MSMQ.Bus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus
{
    public interface IServiceBusRunningService
    {
        Task Run(CancellationToken cancellationToken = default);
    }

    public class ServiceBusRunningService(ILogger<ServiceBusRunningService> _logger, IServiceBusQueueCreator _queueCreator, IServiceBusQueueGenerator _queueGenerator, IQueueSubscriber _queueSubscriber) 
        : IServiceBusRunningService
    {

        public async Task Run(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting services...");

            try
            {
                var queues = _queueGenerator.GetQueues();
                await _queueCreator.CreateQueues(queues, cancellationToken);
                await _queueSubscriber.Subscribe(queues, cancellationToken);
                _logger.LogInformation("Consumer is waiting for messages...");
                while (!cancellationToken.IsCancellationRequested)
                {
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error: '{ErrorMessage}' ocurred.", e.Message);
            }
            finally
            {
                await _queueSubscriber.Unsubscribe(cancellationToken);
                _logger.LogInformation("Stopping service and dependencies...");
            }

        }
    }
}
