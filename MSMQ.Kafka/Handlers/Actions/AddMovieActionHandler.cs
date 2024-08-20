using MSMQ.Kafka.Actions;
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

        protected override Task Handle(AddMovieAction payload, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    public class AddMovieActionHandler2 : KafkaConsumerHandler<AddMovieAction>
    {
        public AddMovieActionHandler2(ILogger<KafkaConsumerHandler<AddMovieAction>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator)
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override Task Handle(AddMovieAction payload, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
