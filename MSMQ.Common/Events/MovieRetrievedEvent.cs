using MSMQ.Common.Entities;
using MSMQ.Common.Events; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Events
{
    public class MovieRetrievedEvent: CommonEvent
    {
        public required Movie Movie { get; init; }
    }
}
