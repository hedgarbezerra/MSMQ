using MSMQ.Common.Events;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Events
{
    public class MovieUpdatedEventHandler : KafkaConsumerHandler<MovieUpdatedEvent>
    {
        public MovieUpdatedEventHandler(ILogger<KafkaConsumerHandler<MovieUpdatedEvent>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) 
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, MovieUpdatedEvent payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Movie '{MovieName}' updated by handler and event was handled here.", payload.Movie.Name);
        }
    }
}
