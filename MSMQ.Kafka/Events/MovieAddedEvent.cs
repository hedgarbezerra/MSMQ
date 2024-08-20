using MSMQ.Common.Events;

namespace MSMQ.Kafka.Events
{
    public class MovieAddedEvent : CommonEvent
    {
        public required Guid MovieId { get; init; }
    }
}
