using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSMQ.Common.Entities;
using MSMQ.Kafka.Actions;
using MSMQ.Kafka.Events;
using MSMQ.Kafka.Factories;
using MSMQ.Kafka.Services;

namespace MSMQ.API.Controllers
{
    [Route("api/kafka")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly IKafkaProducer _producer;

        public KafkaController(IKafkaProducer producer)
        {
            _producer = producer;
        }

        [HttpPost]
        [Route("actions")]
        public  async Task<IActionResult> AddMovieAction([FromBody] Movie movie, CancellationToken cancellationToken = default)
        {
            try
            {
                var addMovieAction = new AddMovieAction() { Movie =  movie };
                await _producer.Publish(addMovieAction, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("actions")]
        public async Task<IActionResult> UpdateMovieAction([FromBody] Movie movie, CancellationToken cancellationToken = default)
        {
            try
            {               
                var updateMovieAction = new UpdateMovieAction() { Movie = movie };
                await _producer.Publish(updateMovieAction, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("actions/{movieId:guid}")]
        public async Task<IActionResult> RemoveMovieAction([FromRoute] Guid movieId, CancellationToken cancellationToken = default)
        {
            try
            {
                var removeMovieAction = new RemoveMovieAction { MovieId = movieId };
                await _producer.Publish(removeMovieAction, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("actions/{movieId:guid}")]
        public async Task<IActionResult> GetMovieAction([FromRoute] Guid movieId, CancellationToken cancellationToken = default)
        {
            try
            {
                var retrieveMovieAction = new GetMovieAction() { MovieId = movieId };
                await _producer.Publish(retrieveMovieAction, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }


        #region Events

        [HttpPost]
        [Route("events")]
        public async Task<IActionResult> AddMovieEvent(CancellationToken cancellationToken = default)
        {
            try
            {
                var @event = new MovieAddedEvent { MovieId = Guid.NewGuid() };
                await _producer.Publish(@event, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("events")]
        public async Task<IActionResult> UpdateMovieEvent(CancellationToken cancellationToken = default)
        {
            try
            {
                var @event = new MovieUpdatedEvent() { Movie = new Movie() };
                await _producer.Publish(@event, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("events")]
        public async Task<IActionResult> RemoveMovieEvent(CancellationToken cancellationToken = default)
        {
            try
            {
                var @event = new MovieRemovedEvent() { MovieId = Guid.NewGuid() };
                await _producer.Publish(@event, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> GetMovieEvent( CancellationToken cancellationToken = default)
        {
            try
            {
                var @event = new MovieRetrievedEvent { Movie = new Movie() };
                await _producer.Publish(@event, cancellationToken);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
        #endregion
    }
}
