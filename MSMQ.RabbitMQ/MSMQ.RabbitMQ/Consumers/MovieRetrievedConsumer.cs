using MSMQ.Common.Actions;
using MSMQ.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Consumers
{
    public class MovieRetrievedConsumer : RabbitConsumer<GetMovieAction>
    {
        private readonly ILogger<RabbitConsumer<GetMovieAction>> _logger;

        public MovieRetrievedConsumer(ILogger<RabbitConsumer<GetMovieAction>> _logger, IRabbitQueuesGenerator _qGenerator) : base(_logger, _qGenerator)
        {
            _logger = _logger;
        }

        protected override async Task Handle(GetMovieAction payload)
        {
            _logger.LogInformation("Handled action to find movie '{MovieId}'", payload.MovieId);
        }
    }
}
