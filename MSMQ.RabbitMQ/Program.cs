using MSMQ.Common;

namespace MSMQ.RabbitMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSerilogOptl(builder.Configuration);
            builder.Services.AddRabbitMqServices(builder.Configuration);
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}