using Grpc.Core;
using MSMQ.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Serializers
{
    public static class MessageSerializer
    {
        public static CommonMessage Deserialize(ReadOnlySpan<byte> data)
        {
            string json = Encoding.UTF8.GetString(data);
            var tempMessage = JsonConvert.DeserializeObject<CommonMessage>(json);
            if (tempMessage is null)
                throw new InvalidOperationException($"Unable to deserialize message to {nameof(CommonMessage)}");

            string payloadJson = tempMessage?.Payload?.ToString() ?? throw new InvalidCastException("Unable to convert message payload ");
            Type type = Type.GetType(tempMessage.PayloadType, throwOnError: true);
            var payload = JsonConvert.DeserializeObject(payloadJson, type);

            return CommonMessage.Create(tempMessage.Id, payload);
        }

        public static byte[] Serialize(CommonMessage data)
        {
            var json = JsonConvert.SerializeObject(data);

            return Encoding.UTF8.GetBytes(json);
        }
    }
}
