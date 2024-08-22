using MSMQ.Common.Actions;
using MSMQ.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Actions
{
    public class UpdateMovieAction : CommonAction
    {
        public Movie Movie { get; init; }
    }
}
