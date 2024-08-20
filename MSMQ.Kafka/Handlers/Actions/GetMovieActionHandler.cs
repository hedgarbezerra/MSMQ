using MSMQ.Kafka.Actions;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers.Actions
{
    public class GetMovieActionHandler : KafkaConsumerHandler<GetMovieAction>
    {
        public GetMovieActionHandler(ILogger<KafkaConsumerHandler<GetMovieAction>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator) 
            : base(logger, producer, kafkaTopicGenerator)
        {
        }

        protected override Task Handle(GetMovieAction payload, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
