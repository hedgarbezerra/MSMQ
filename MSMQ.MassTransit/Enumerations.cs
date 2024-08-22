using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.MassTransit
{
    public static class Enumerations
    {

        public enum EBrokerType
        {
            Kafka,
            RabbitMQ,
            ServiceBus
        }
    }
}
