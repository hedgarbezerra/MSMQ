using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Exceptions
{
    public class ConsumerNotImplementedException : Exception
    {
        public ConsumerNotImplementedException(string topic) : base("The selected topic does not have a handler implemented.")
        {
            Topic = topic;
        }

        public string Topic { get; init; }
    }
}
