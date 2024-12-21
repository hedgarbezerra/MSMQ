using Confluent.Kafka;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MSMQ.Common.Actions;
using MSMQ.Common.Entities;
using MSMQ.Common.Messages;
using MSMQ.IntegrationTests.RabbitMq;
using MSMQ.Kafka.Factories;
using MSMQ.Kafka.Services;
using MSMQ.RabbitMQ.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace MSMQ.IntegrationTests.Kafka
{
    [TestFixture]
    public class KafkaTesting
    {
        private WebAppFactory _appFactory;
        private HttpClient _client;
        private IConsumer<Null, CommonMessage> _consumer;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _appFactory = new WebAppFactory();
            await _appFactory.StartContainers();

            _client = _appFactory.CreateClient();

            using var scope = _appFactory.Services.CreateScope();

            KafkaTopicsInitializer.Initialize(scope.ServiceProvider, out var topics);

            var consumerFactory = scope.ServiceProvider.GetRequiredService<IKafkaConsumerFactory>();
            _consumer = consumerFactory.Create();
            _consumer.Subscribe(topics.Select(t => t.Name));
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _appFactory.StopContainers();
            _appFactory.Dispose();
            _client.Dispose();
            _consumer.Dispose();
        }

        [Test]
        public async Task MovieRequestedById_MessageShouldBeConsumed()
        {
            Guid expectedMovieId = Guid.NewGuid();
            var requestResult = await _client.GetAsync($"api/kafka/actions/{expectedMovieId.ToString()}");

            requestResult.EnsureSuccessStatusCode();

            var consumeResult = _consumer.Consume();

            Assert.IsNotNull(consumeResult);
            Assert.IsNotNull(consumeResult.Message.Value);

            var action = consumeResult.Message.Value.Payload as GetMovieAction;
            Assert.IsNotNull(action);
            Assert.AreEqual(expectedMovieId, action.MovieId);
        }
        
        [Test]
        public async Task MovieRemovedById_MessageShouldBeConsumed()
        {
            Guid expectedMovieId = Guid.NewGuid();
            var requestResult = await _client.DeleteAsync($"api/kafka/actions/{expectedMovieId.ToString()}");

            requestResult.EnsureSuccessStatusCode();

            var consumeResult = _consumer.Consume();

            Assert.IsNotNull(consumeResult);
            Assert.IsNotNull(consumeResult.Message.Value);

            var action = consumeResult.Message.Value.Payload as RemoveMovieAction;
            action.Should().NotBeNull();
            action.MovieId.Should().Be(expectedMovieId);
        }


        [Test]
        public async Task MovieAdded_MessageShouldBeConsumed()
        {
            var movie = new Movie() { Name = "foo", Release = DateTime.UtcNow, Starring = [] };
            var requestResult = await _client.PostAsJsonAsync($"api/kafka/actions", movie);

            requestResult.EnsureSuccessStatusCode();

            var consumeResult = _consumer.Consume();

            Assert.IsNotNull(consumeResult);
            Assert.IsNotNull(consumeResult.Message.Value);

            var action = consumeResult.Message.Value.Payload as AddMovieAction;
            action.Should().NotBeNull();
            action.Movie.Should()
                .NotBeNull()
                .And.BeEquivalentTo(movie);
        }

        [Test]
        public async Task MovieUpdated_MessageShouldBeConsumed()
        {
            var movie = new Movie() { Name = "foo", Release = DateTime.UtcNow, Starring = [] };
            var requestResult = await _client.PutAsJsonAsync($"api/kafka/actions", movie);

            requestResult.EnsureSuccessStatusCode();

            var consumeResult = _consumer.Consume();

            Assert.IsNotNull(consumeResult);
            Assert.IsNotNull(consumeResult.Message.Value);

            var action = consumeResult.Message.Value.Payload as UpdateMovieAction;
            action.Should().NotBeNull();
            action.Movie.Should()
                .NotBeNull()
                .And.BeEquivalentTo(movie);
        }
    }
}
