using MSMQ.Kafka.Events;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Events
{
    public class MovieRetrievedEventHandler : KafkaConsumerHandler<MovieRetrievedEvent>
    {
        public MovieRetrievedEventHandler(ILogger<KafkaConsumerHandler<MovieRetrievedEvent>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator)
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override async Task Handle(Guid sourceId, MovieRetrievedEvent payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Movie '{MovieName}' received from handler and event was handled here.", payload.Movie.Name);
        }
    }
}
