using MSMQ.Common.Actions;
using MSMQ.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Consumers
{
    public class MovieAddedConsumer : RabbitConsumer<AddMovieAction>
    {
        public MovieAddedConsumer(ILogger<RabbitConsumer<AddMovieAction>> _logger, IRabbitQueuesGenerator _qGenerator) : base(_logger, _qGenerator)
        {
        }

        protected override async Task Handle(AddMovieAction payload)
        {
            _logger.LogInformation("Handled action to add movie '{MovieName}'", payload.Movie.Name);
        }
    }
}
