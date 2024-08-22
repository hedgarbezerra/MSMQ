using MSMQ.Common.Actions;
using MSMQ.Common.Events;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Actions
{
    public class RemoveMovieActionHandler : KafkaConsumerHandler<RemoveMovieAction>
    {
        public RemoveMovieActionHandler(ILogger<KafkaConsumerHandler<RemoveMovieAction>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) 
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, RemoveMovieAction payload, CancellationToken cancellationToken = default)
        {
            var @event = new MovieRemovedEvent() { MovieId = payload.MovieId, SourceId = sourceId };
            await _producer.Publish(@event, cancellationToken);

            _logger.LogInformation("Movie #'{MovieId}' was removed to database by handler.", payload.MovieId);
        }
    }
}
