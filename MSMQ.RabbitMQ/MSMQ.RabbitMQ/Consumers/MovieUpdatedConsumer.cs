using MSMQ.Common.Actions;
using MSMQ.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Consumers
{
    public class MovieUpdatedConsumer : RabbitConsumer<UpdateMovieAction>
    {
        private readonly ILogger<RabbitConsumer<UpdateMovieAction>> _logger;

        public MovieUpdatedConsumer(ILogger<RabbitConsumer<UpdateMovieAction>> _logger, IRabbitQueuesGenerator _qGenerator) : base(_logger, _qGenerator)
        {
            _logger = _logger;
        }

        protected override async Task Handle(UpdateMovieAction payload)
        {
            _logger.LogInformation("Handled action to update movie '{MovieName}'", payload.Movie.Name);
        }
    }
}
