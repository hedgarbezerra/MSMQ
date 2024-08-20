
namespace MSMQ.Bus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<BusWorker>();
            //builder.Services.AddSerilogSeq(builder.Configuration);

            var host = builder.Build();
            host.Run();
        }
    }
}