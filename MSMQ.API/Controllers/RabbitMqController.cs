using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSMQ.Common.Actions;
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

        [HttpGet]
        public IActionResult Get() 
        {
            var action = new GetMovieAction();
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            var action = new AddMovieAction();
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update()
        {
            var action = new UpdateMovieAction();
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove()
        {
            var action = new RemoveMovieAction();
            var message = CommonMessage.Create(action);

            _producer.Publish(message, CancellationToken.None);

            return Ok();
        }
    }
}
