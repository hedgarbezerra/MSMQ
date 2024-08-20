using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Actions
{
    public class CommonAction
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTimeOffset Time { get; init; } = DateTimeOffset.Now;
    }
}
