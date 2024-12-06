using MSMQ.Common.Actions;
using MSMQ.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Consumers
{
    public class MovieRemovedConsumer : RabbitConsumer<RemoveMovieAction>
    {
        public MovieRemovedConsumer(ILogger<RabbitConsumer<RemoveMovieAction>> _logger, IRabbitQueuesGenerator _qGenerator) : base(_logger, _qGenerator)
        {
        }

        protected override async Task Handle(RemoveMovieAction payload)
        {
            _logger.LogInformation("Handled action to remove movie '{MovieId}'", payload.MovieId);
        }
    }

    public class MovieRemovedConsumer2 : RabbitConsumer<RemoveMovieAction>
    {
        public MovieRemovedConsumer2(ILogger<RabbitConsumer<RemoveMovieAction>> _logger, IRabbitQueuesGenerator _qGenerator) : base(_logger, _qGenerator)
        {
        }

        protected override async Task Handle(RemoveMovieAction payload)
        {
            _logger.LogInformation("Handled action to remove movie '{MovieId}'", payload.MovieId);
        }
    }
}
