using MSMQ.Bus.Services;
using MSMQ.Common.Actions;
using MSMQ.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus.Handlers
{
    public class AddMovieMessageHandler : MessageHandler<AddMovieAction>
    {
        public AddMovieMessageHandler(ILogger<MessageHandler<AddMovieAction>> logger, IServiceBusQueueGenerator queueGenerator, IMessagePublisher messagePublisher) 
            : base(logger, queueGenerator, messagePublisher)
        {
        }

        protected override async Task Handle(AddMovieAction payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Message received from queue '{QueueName}' and finished handling.", Queue);

            var @event = new MovieAddedEvent() { MovieId = payload.Movie.Id, SourceId = Guid.NewGuid() };

            await _messagePublisher.Publish(@event);
            _logger.LogInformation("Queue '{QueueName}' published and event after processing.", Queue);
        }
    }
}
