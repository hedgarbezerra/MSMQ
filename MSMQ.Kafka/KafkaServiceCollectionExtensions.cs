using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSMQ.Kafka.Events;
using MSMQ.Kafka.Factories;
using MSMQ.Kafka.Handlers;
using MSMQ.Kafka.Handlers.Actions;
using MSMQ.Kafka.Handlers.Events;
using MSMQ.Kafka.Services;

namespace MSMQ.Kafka
{
    public static class KafkaServiceCollectionExtensions
    {
        public static void AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IKafkaAdminClientFactory, KafkaAdminClientFactory>();
            services.AddScoped<IKafkaConsumerFactory, KafkaConsumerFactory>();
            services.AddScoped<IKafkaProducerFactory, KafkaProducerFactory>();
            services.AddScoped<IKafkaConsumerHandlerFactory, KafkaConsumerHandlerFactory>();

            services.AddScoped<IKafkaConsumerHandler, AddMovieActionHandler2>();
            services.AddScoped<IKafkaConsumerHandler, AddMovieActionHandler>();
            services.AddScoped<IKafkaConsumerHandler, GetMovieActionHandler>();
            services.AddScoped<IKafkaConsumerHandler, RemoveMovieActionHandler>();
            services.AddScoped<IKafkaConsumerHandler, UpdateMovieActionHandler>();
            services.AddScoped<IKafkaConsumerHandler, MovieAddedEventHandler>();
            services.AddScoped<IKafkaConsumerHandler, MovieRemovedEventHandler>();
            services.AddScoped<IKafkaConsumerHandler, MovieRetrievedEventHandler>();
            services.AddScoped<IKafkaConsumerHandler, MovieUpdatedEventHandler>();

            services.AddScoped<IKafkaTopicCreator, KafkaTopicCreator>();
            services.AddScoped<IKafkaTopicGenerator, KafkaTopicGenerator>();
            services.AddScoped<IKafkaProducer, KafkaProducer>();
            services.AddScoped<IKafkaRunningService, KafkaRunningService>();
        }
    }
}
