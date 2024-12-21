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

        public RabbitMqController(IRabbitProducer producer)
        {
            _producer = producer;
        }

        [HttpGet, Route("{id:guid}")]
        public IActionResult Get([FromRoute]Guid id)
        {
            var action = new GetMovieAction() { MovieId = id };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody]Movie movie)
        {
            var action = new AddMovieAction() { Movie = movie };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Movie movie)
        {
            var action = new UpdateMovieAction() { Movie = movie };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpDelete, Route("{id:guid}")]
        public IActionResult Remove([FromRoute] Guid id)
        {
            var action = new RemoveMovieAction() { MovieId = id };
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }
    }
}
