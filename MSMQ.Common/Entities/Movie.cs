using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Entities
{
    public class Movie
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTime Release { get; init; }
        public IReadOnlyCollection<string> Starring { get; init; }
    }
}
