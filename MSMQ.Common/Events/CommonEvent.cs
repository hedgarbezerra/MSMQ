using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Events
{
    public class CommonEvent
    {
        public required Guid SourceId { get; init; }
    }
}
