using MSMQ.Common.Entities;
using MSMQ.Kafka.Actions;
using MSMQ.Kafka.Events;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Actions
{
    public class UpdateMovieActionHandler : KafkaConsumerHandler<UpdateMovieAction>
    {
        public UpdateMovieActionHandler(ILogger<KafkaConsumerHandler<UpdateMovieAction>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) 
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, UpdateMovieAction payload, CancellationToken cancellationToken = default)
        {
            var movie = new Movie() { Name = "Carlitos" };
            var @event = new MovieUpdatedEvent() { Movie = movie, SourceId = sourceId };
            await _producer.Publish(@event, cancellationToken);

            _logger.LogInformation("Movie '{MovieName}' was updated by handler.", movie.Name);
        }
    }
}
