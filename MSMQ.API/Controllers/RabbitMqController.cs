using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSMQ.Common.Actions;
using MSMQ.Common.Entities;
using MSMQ.Common.Messages;
using MSMQ.RabbitMQ.Services;

namespace MSMQ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitProducer _producer;

        private static Movie Movie = new Common.Entities.Movie() { Name = "Top" };

        public RabbitMqController(IRabbitProducer producer)
        {
            _producer = producer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var action = new GetMovieAction() { MovieId = Movie.Id };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            var action = new AddMovieAction() { Movie = Movie };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update()
        {
            var action = new UpdateMovieAction() { Movie = Movie };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove()
        {
            var action = new RemoveMovieAction() { MovieId = Movie.Id };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }
    }
}
