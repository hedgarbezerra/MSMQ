using MSMQ.Kafka;
using MSMQ.Common;

namespace MSMQ.Kafka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSerilogOptl(builder.Configuration);
            builder.Services.AddKafkaServices(builder.Configuration);
            builder.Services.AddHostedService<KafkaWorker>();

            var host = builder.Build();
            host.Run();
        }
    }
}