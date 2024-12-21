using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MSMQ.Common.Actions;
using MSMQ.Common.Entities;
using MSMQ.Common.Messages;
using MSMQ.Common.Serializers;
using MSMQ.RabbitMQ.Factories;
using MSMQ.RabbitMQ.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.IntegrationTests.RabbitMq
{
    [TestFixture]
    public class RabbitMqTesting
    {
        private WebAppFactory _appFactory;
        private HttpClient _client;
        private IConnection _rabbitmqConnection;
        private IRabbitQueuesGenerator _rabbitMqQueueGenerator;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _appFactory = new WebAppFactory();
            await _appFactory.StartContainers();

            _client = _appFactory.CreateClient();

            var scope = _appFactory.Services.CreateScope();
            RabbitMqQueuesInitializer.Initialize(scope.ServiceProvider, out List<string> queues);

            _rabbitMqQueueGenerator = scope.ServiceProvider.GetRequiredService<IRabbitQueuesGenerator>();
            IRabbitMqConnectionFactory rabbitConsumerFactory = scope.ServiceProvider.GetRequiredService<IRabbitMqConnectionFactory>();
            _rabbitmqConnection = rabbitConsumerFactory.Create();

        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _rabbitmqConnection.Dispose();
            await _appFactory.StopContainers();
            _appFactory.Dispose();
            _client.Dispose();
        }

        [Test]
        public async Task GetMovieById_ShouldProduceMessageOnQueue_MessageShouldHaveSameDataFromRequest()
        {
            //Arrange
            string expectedQueueName = _rabbitMqQueueGenerator.GetQueue(typeof(GetMovieAction));
            Guid expectedMovieId = Guid.NewGuid();

            //Act
            var requestResult = await _client.GetAsync($"api/rabbitmq/{expectedMovieId.ToString()}");

            //Assert
            requestResult.EnsureSuccessStatusCode();

            using var channel = _rabbitmqConnection.CreateModel();
            var queueMessageResult = channel.BasicGet(expectedQueueName, true);
            queueMessageResult.RoutingKey.Should()
                .Be(expectedQueueName);

            var message = MessageSerializer.Deserialize(queueMessageResult.Body.Span);
            message.Should().NotBeNull()
                .And.BeAssignableTo<CommonMessage>();
            message.Payload.Should().NotBeNull();
            message.PayloadType.Should().Be(typeof(GetMovieAction).AssemblyQualifiedName);

            GetMovieAction payload = message.Payload as GetMovieAction;
            payload.Should().NotBeNull();
            payload.MovieId.Should().Be(expectedMovieId);
        }


        [Test]
        public async Task RemoveMovieById_ShouldProduceMessageOnQueue_MessageShouldHaveSameDataFromRequest()
        {
            //Arrange
            string expectedQueueName = _rabbitMqQueueGenerator.GetQueue(typeof(RemoveMovieAction));
            Guid expectedMovieId = Guid.NewGuid();

            //Act
            var requestResult = await _client.DeleteAsync($"api/rabbitmq/{expectedMovieId.ToString()}");

            //Assert
            requestResult.EnsureSuccessStatusCode();

            using var channel = _rabbitmqConnection.CreateModel();
            var queueMessageResult = channel.BasicGet(expectedQueueName, true);
            queueMessageResult.RoutingKey.Should()
                .Be(expectedQueueName);

            var message = MessageSerializer.Deserialize(queueMessageResult.Body.Span);
            message.Should().NotBeNull()
                .And.BeAssignableTo<CommonMessage>();
            message.Payload.Should().NotBeNull();
            message.PayloadType.Should().Be(typeof(RemoveMovieAction).AssemblyQualifiedName);

            RemoveMovieAction payload = message.Payload as RemoveMovieAction;
            payload.Should().NotBeNull();
            payload.MovieId.Should().Be(expectedMovieId);
        }


        [Test]
        public async Task AddMovie_ShouldProduceMessageOnQueue_MessageShouldHaveSameDataFromRequest()
        {
            //Arrange
            string expectedQueueName = _rabbitMqQueueGenerator.GetQueue(typeof(AddMovieAction));
            Movie newMovie = new Movie { Name = "Unknown", Release = new DateTime(2020, 01, 01), Starring = [] };

            //Act
            var requestResult = await _client.PostAsJsonAsync($"api/rabbitmq", newMovie);

            //Assert
            requestResult.EnsureSuccessStatusCode();

            using var channel = _rabbitmqConnection.CreateModel();
            var queueMessageResult = channel.BasicGet(expectedQueueName, true);
            queueMessageResult.RoutingKey.Should()
                .Be(expectedQueueName);

            var message = MessageSerializer.Deserialize(queueMessageResult.Body.Span);
            message.Should().NotBeNull()
                .And.BeAssignableTo<CommonMessage>();
            message.Payload.Should().NotBeNull();
            message.PayloadType.Should().Be(typeof(AddMovieAction).AssemblyQualifiedName);

            AddMovieAction payload = message.Payload as AddMovieAction;
            payload.Should().NotBeNull();
            payload.Movie.Should().BeEquivalentTo(newMovie);
        }
        
        [Test]
        public async Task UpdateMovie_ShouldProduceMessageOnQueue_MessageShouldHaveSameDataFromRequest()
        {
            //Arrange
            string expectedQueueName = _rabbitMqQueueGenerator.GetQueue(typeof(UpdateMovieAction));
            Movie updatedMovie = new Movie { Name = "Unknown", Release = new DateTime(2020, 01, 01), Starring = [] };

            //Act
            var requestResult = await _client.PutAsJsonAsync($"api/rabbitmq", updatedMovie);

            //Assert
            requestResult.EnsureSuccessStatusCode();

            using var channel = _rabbitmqConnection.CreateModel();
            var queueMessageResult = channel.BasicGet(expectedQueueName, true);
            queueMessageResult.RoutingKey.Should()
                .Be(expectedQueueName);

            var message = MessageSerializer.Deserialize(queueMessageResult.Body.Span);
            message.Should().NotBeNull()
                .And.BeAssignableTo<CommonMessage>();
            message.Payload.Should().NotBeNull();
            message.PayloadType.Should().Be(typeof(UpdateMovieAction).AssemblyQualifiedName);

            UpdateMovieAction payload = message.Payload as UpdateMovieAction;
            payload.Should().NotBeNull();
            payload.Movie.Should().BeEquivalentTo(updatedMovie);
        }

    }
}
