using MSMQ.Common.Entities;
using MSMQ.Common.Events;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Events
{
    public class MovieAddedEventHandler : KafkaConsumerHandler<MovieAddedEvent>
    {
        public MovieAddedEventHandler(ILogger<KafkaConsumerHandler<MovieAddedEvent>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, MovieAddedEvent payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Movie #{MovieId} added by handler and event was handled here.", payload.MovieId);
        }
    }
}
