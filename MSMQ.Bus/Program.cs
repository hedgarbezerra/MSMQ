namespace MSMQ.Bus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddTelemetry(builder.Configuration);

            var host = builder.Build();
            host.Run();
        }
    }
}