using Confluent.Kafka;
using MSMQ.Common.Messages;
using MSMQ.Common.Serializers;
using Newtonsoft.Json;
using System.Text;

namespace MSMQ.Kafka.Services
{
    public class KafkaMessageSerializer : ISerializer<CommonMessage>, IDeserializer<CommonMessage>
    {
        public CommonMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
                throw new InvalidOperationException("Message binary cannot be null or empty.");
           
            return MessageSerializer.Deserialize(data);
        }

        public byte[] Serialize(CommonMessage data, SerializationContext context) => MessageSerializer.Serialize(data);
    }
}
