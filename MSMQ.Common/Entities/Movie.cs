using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Common.Entities
{
    public class Movie
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Name { get; init; }
        public DateTime Release { get; init; }
        public IReadOnlyCollection<string> Starring { get; init; }
    }
}
