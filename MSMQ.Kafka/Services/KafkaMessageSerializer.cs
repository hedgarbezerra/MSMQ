using Confluent.Kafka;
using MSMQ.Kafka.Messages;
using Newtonsoft.Json;
using System.Text;

namespace MSMQ.Kafka.Services
{
    public class KafkaMessageSerializer : ISerializer<KafkaMessage>, IDeserializer<KafkaMessage>
    {
        public KafkaMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            string json = Encoding.UTF8.GetString(data);
            var tempMessage = JsonConvert.DeserializeObject<KafkaMessage>(json);
            if (tempMessage is null)
                throw new InvalidOperationException($"Unable to deserialize message to {nameof(KafkaMessage)}");

            string payloadJson = tempMessage?.Payload?.ToString() ?? throw new InvalidCastException("Unable to convert message payload ");
            Type type = Type.GetType(tempMessage.PayloadType, throwOnError: true);
            var payload = JsonConvert.DeserializeObject(payloadJson, type);

            return KafkaMessage.Create(tempMessage.Id, payload);
        }

        public byte[] Serialize(KafkaMessage data, SerializationContext context)
        {
            var json = JsonConvert.SerializeObject(data);

            return Encoding.UTF8.GetBytes(json);
        }
    }
}
