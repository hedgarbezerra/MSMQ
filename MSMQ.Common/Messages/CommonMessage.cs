using Newtonsoft.Json;
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

        [JsonConstructor]
        private CommonMessage(Guid id, DateTimeOffset time, object payload, string payloadType)
        {
            Id = id;
            Time = time;
            Payload = payload;
            PayloadType = payloadType;
        }
        public static CommonMessage Create(object payload) => new CommonMessage(Guid.NewGuid(), DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static CommonMessage Create(Guid id, object payload) => new CommonMessage(id, DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static CommonMessage Create(object payload, string payloadType) => new CommonMessage(Guid.NewGuid(), DateTimeOffset.Now, payload, payloadType);
        public static CommonMessage Create(Guid id, object payload, string payloadType) => new CommonMessage(id, DateTimeOffset.Now, payload, payloadType);
        public static CommonMessage Create(Guid id, DateTimeOffset time, object payload, string payloadType) => new CommonMessage(id, time, payload, payloadType);

    }

    public class CommonMessage<T>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTimeOffset Time { get; init; } = DateTimeOffset.Now;
        public T Payload { get; init; }
        public string PayloadType { get; set; }

        [JsonConstructor]
        private CommonMessage(Guid id, DateTimeOffset time, T payload, string payloadType)
        {
            Id = id;
            Time = time;
            Payload = payload;
            PayloadType = payloadType;
        }

        public static CommonMessage<T> Create(T payload) => new CommonMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static CommonMessage<T> Create(Guid id, T payload) => new CommonMessage<T>(id, DateTimeOffset.Now, payload, payload.GetType().AssemblyQualifiedName);
        public static CommonMessage<T> Create(T payload, string payloadType) => new CommonMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload, payloadType);
        public static CommonMessage<T> Create(Guid id, T payload, string payloadType) => new CommonMessage<T>(id, DateTimeOffset.Now, payload, payloadType);
        public static CommonMessage Create(Guid id, DateTimeOffset time, T payload, string payloadType) => new CommonMessage<T>(id, time, payload, payloadType);

        public static implicit operator CommonMessage(CommonMessage<T> message) =>
            message switch
            {
                null => throw new ArgumentNullException(nameof(message)),
                { Payload: not null and T type } => CommonMessage.Create(message.Id, message.Time, message.Payload, message.PayloadType),
                _ => throw new InvalidCastException($"Payload cannot be cast to type {typeof(T).AssemblyQualifiedName}.")
            };
        
        public static implicit operator CommonMessage<T>(CommonMessage message) =>
            message switch
            {
                null => throw new ArgumentNullException(nameof(message)),
                { Payload: not null and T type } => CommonMessage<T>.Create(message.Id, message.Time, (T)message.Payload, message.PayloadType),
                _ => throw new InvalidCastException($"Payload cannot be cast to type {typeof(T).AssemblyQualifiedName}.")
            };
    }
}
