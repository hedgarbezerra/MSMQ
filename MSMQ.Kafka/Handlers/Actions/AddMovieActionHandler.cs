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
    public class AddMovieActionHandler : KafkaConsumerHandler<AddMovieAction>
    {
        public AddMovieActionHandler(ILogger<KafkaConsumerHandler<AddMovieAction>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) 
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, AddMovieAction payload, CancellationToken cancellationToken = default)
        {
            var @event = new MovieAddedEvent() { MovieId = payload.Movie.Id, SourceId = sourceId };
            await _producer.Publish(@event, cancellationToken);

            _logger.LogInformation("Movie '{MovieName}' added to database by handler.", payload.Movie.Name);
        }
    }
    public class AddMovieActionHandler2 : KafkaConsumerHandler<AddMovieAction>
    {
        public AddMovieActionHandler2(ILogger<KafkaConsumerHandler<AddMovieAction>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator)
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, AddMovieAction payload, CancellationToken cancellationToken = default)
        {
            var @event = new MovieAddedEvent() { MovieId = payload.Movie.Id, SourceId = sourceId };
            await _producer.Publish(@event, cancellationToken);

            _logger.LogInformation("Movie '{MovieName}' added to database by handler 2.", payload.Movie.Name);
        }
    }
}
