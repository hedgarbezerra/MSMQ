using MSMQ.Kafka;
using MSMQ.RabbitMQ;
using MSMQ.Bus;
using MSMQ.Common;
using MSMQ.Common.Extensions;
namespace MSMQ.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddMultiplayerOpenTelemetry(builder.Configuration);
            // Add services to the container.

            //builder.Services.AddSerilogOptl(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddKafkaServices(builder.Configuration);
            builder.Services.AddRabbitMqServices(builder.Configuration);
            builder.Services.AddServiceBus(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
