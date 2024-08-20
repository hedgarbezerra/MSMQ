using MSMQ.Kafka.Events;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Events
{
    public class MovieRemovedEventHandler : KafkaConsumerHandler<MovieRemovedEvent>
    {
        public MovieRemovedEventHandler(ILogger<KafkaConsumerHandler<MovieRemovedEvent>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) 
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override Task Handle(MovieRemovedEvent payload, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
