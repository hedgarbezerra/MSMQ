using MSMQ.Common.Events;

namespace MSMQ.Common.Events
{
    public class MovieAddedEvent : CommonEvent
    {
        public required Guid MovieId { get; init; }
    }
}
