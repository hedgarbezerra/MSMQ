using MSMQ.Common.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Actions
{
    public class GetMovieAction : CommonAction
    {
        public Guid MovieId { get; init; }
    }
}
