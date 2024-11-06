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
    public class RemoveMovieMovieHandler : MessageHandler<RemoveMovieAction>
    {
        public RemoveMovieMovieHandler(ILogger<MessageHandler<RemoveMovieAction>> logger, IServiceBusQueueGenerator queueGenerator, IMessagePublisher messagePublisher)
            : base(logger, queueGenerator, messagePublisher)
        {
        }

        protected override async Task Handle(RemoveMovieAction payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Message received from queue '{QueueName}' and finished handling.", Queue);

            var @event = new MovieRemovedEvent() { MovieId = payload.MovieId, SourceId = Guid.NewGuid() };

            await _messagePublisher.Publish(@event);
            _logger.LogInformation("Queue '{QueueName}' published and event after processing.", Queue);
        }
    }
}
