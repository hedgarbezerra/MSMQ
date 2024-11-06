using MSMQ.Bus.Services;
using MSMQ.Common.Actions;
using MSMQ.Common.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus.Handlers
{
    public class MovieAddedEventHandler : MessageHandler<MovieAddedEvent>
    {
        public MovieAddedEventHandler(ILogger<MessageHandler<MovieAddedEvent>> logger, IServiceBusQueueGenerator queueGenerator, IMessagePublisher messagePublisher)
            : base(logger, queueGenerator, messagePublisher)
        {
        }

        protected override async Task Handle(MovieAddedEvent payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Message received from queue '{QueueName}' and finished handling.", Queue);
        }
    }
}
