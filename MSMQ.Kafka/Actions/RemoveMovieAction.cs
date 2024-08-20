using MSMQ.Common.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Actions
{
    public class RemoveMovieAction : CommonAction
    {
        public Guid MovieId { get; init; }
    }
}
