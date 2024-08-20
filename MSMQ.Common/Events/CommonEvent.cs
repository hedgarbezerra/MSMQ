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
        public Guid SourceId { get; init; } = Guid.NewGuid();
        public DateTimeOffset Time { get; init; } = DateTimeOffset.Now;
    }
}
