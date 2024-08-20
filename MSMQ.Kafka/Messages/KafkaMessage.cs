using MSMQ.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Messages
{
    public class KafkaMessage : CommonMessage
    {
        public string PayloadType { get; set; }
        public KafkaMessage(Guid id, DateTimeOffset time, object payload, string payloadType) : base(id, time, payload)
        {
            PayloadType = payloadType;
        }
        public static new KafkaMessage Create(object payload) => new KafkaMessage(Guid.NewGuid(), DateTimeOffset.Now, payload, payload.GetType().FullName);
        public static new KafkaMessage Create(Guid id, object payload) => new KafkaMessage(id, DateTimeOffset.Now, payload, payload.GetType().FullName);
        public static new KafkaMessage Create(object payload, string payloadType) => new KafkaMessage(Guid.NewGuid(), DateTimeOffset.Now, payload, payloadType);
        public static new KafkaMessage Create(Guid id, object payload, string payloadType) => new KafkaMessage(id, DateTimeOffset.Now, payload, payloadType);
    }

    public class KafkaMessage<T> : KafkaMessage
    {
        public KafkaMessage(Guid id, DateTimeOffset time, T payload, string payloadType) : base(id, time, payload, payloadType)
        {
        }
        public static new KafkaMessage<T> Create(T payload) => new KafkaMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload, payload.GetType().FullName);
        public static new KafkaMessage<T> Create(Guid id, T payload) => new KafkaMessage<T>(id, DateTimeOffset.Now, payload, payload.GetType().FullName);
        public static new KafkaMessage<T> Create(T payload, string payloadType) => new KafkaMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload, payloadType);
        public static new KafkaMessage<T> Create(Guid id, T payload, string payloadType) => new KafkaMessage<T>(id, DateTimeOffset.Now, payload, payloadType);
    }
}
