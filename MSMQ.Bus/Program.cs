using MSMQ.Common;

namespace MSMQ.Bus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSerilogSeq(builder.Configuration);
            builder.Services.AddServiceBus(builder.Configuration);

            builder.Services.AddHostedService<BusWorker>();

            var host = builder.Build();
            host.Run();
        }
    }
}