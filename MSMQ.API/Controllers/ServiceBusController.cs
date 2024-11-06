using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSMQ.Bus.Services;
using MSMQ.Common.Actions;
using MSMQ.Common.Entities;

namespace MSMQ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceBusController(IMessagePublisher _publisher) : ControllerBase
    {
        [HttpPost]
        public IActionResult Add()
        {
            var action = new AddMovieAction() { Movie = new Movie() { Name = "Movie" } };
            _publisher.Publish(action);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove()
        {
            var action = new RemoveMovieAction() { MovieId = Guid.NewGuid()};
            _publisher.Publish(action);

            return Ok();
        }
    }
}
