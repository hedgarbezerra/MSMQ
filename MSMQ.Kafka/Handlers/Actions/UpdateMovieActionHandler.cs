using MSMQ.Kafka.Actions;
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

        protected override Task Handle(UpdateMovieAction payload, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
