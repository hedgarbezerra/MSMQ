﻿using MSMQ.Common.Messages;

namespace MSMQ.Common
{
    public static class Utilities
    {
        public static T GetPayload<T>(this CommonMessage message) => (T)message.Payload;
        public static Type GetPayloadType(this CommonMessage message) => message.Payload.GetType();
    }
}