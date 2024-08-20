using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Events
{
    public class CommonEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required Guid SourceId { get; init; }
        public DateTimeOffset Time { get; init; } = DateTimeOffset.Now;
    }
}
