using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Messages
{
    public class CommonMessage
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTimeOffset Time { get; init; } = DateTimeOffset.Now;
        public object Payload { get; init; }
        public string PayloadType { get; set; }
        public CommonMessage(Guid id, DateTimeOffset time, object payload, string payloadType)
        {
            Id = id;
            Time = time;
            Payload = payload;
            PayloadType = payloadType;
        }
        public static new CommonMessage Create(object payload) => new CommonMessage(Guid.NewGuid(), DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static new CommonMessage Create(Guid id, object payload) => new CommonMessage(id, DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static new CommonMessage Create(object payload, string payloadType) => new CommonMessage(Guid.NewGuid(), DateTimeOffset.Now, payload, payloadType);
        public static new CommonMessage Create(Guid id, object payload, string payloadType) => new CommonMessage(id, DateTimeOffset.Now, payload, payloadType);
    }

    public class CommonMessage<T> : CommonMessage
    {
        public CommonMessage(Guid id, DateTimeOffset time, T payload, string payloadType) : base(id, time, payload, payloadType)
        {
        }
        public static new CommonMessage<T> Create(T payload) => new CommonMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static new CommonMessage<T> Create(Guid id, T payload) => new CommonMessage<T>(id, DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static new CommonMessage<T> Create(T payload, string payloadType) => new CommonMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload, payloadType);
        public static new CommonMessage<T> Create(Guid id, T payload, string payloadType) => new CommonMessage<T>(id, DateTimeOffset.Now, payload, payloadType);
    }
}
