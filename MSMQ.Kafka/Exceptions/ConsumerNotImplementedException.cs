using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Exceptions
{
    public class ConsumerNotImplementedException : Exception
    {
        public string Topic { get; init; }
        public Guid MessageId { get; init; }
        public ConsumerNotImplementedException(Guid messageId, string topic) : base("The selected topic does not have a handler implemented.")
        {
            MessageId = messageId;
            Topic = topic;
        }

    }
}
