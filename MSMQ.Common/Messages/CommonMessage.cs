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

        public CommonMessage(Guid id,  DateTimeOffset time, object payload)
        {
            Id = id;
            Time = time;
            Payload = payload;
        }
        public static CommonMessage<T> Create<T>(T payload) => new CommonMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload);
    }

    public class CommonMessage<T> : CommonMessage
    {
        public CommonMessage(Guid id, DateTimeOffset time, T payload) : base(id, time, payload)
        {
        }
        public static CommonMessage<T> Create(T payload) => new CommonMessage<T>(Guid.NewGuid(), DateTimeOffset.Now, payload);
    }
}
