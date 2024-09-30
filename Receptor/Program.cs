using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Receptor.Application.Services;
using Receptor.Infrastructure.Database;
using Receptor.Infrastructure.Interfaces;
using Receptor.Infrastructure.Kafka;

namespace Receptor;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("30 seconds delay");
        await Task.Delay(TimeSpan.FromSeconds(30));
        Console.WriteLine("Started");

        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(hostContext.Configuration);

                services.AddHostedService<PrintJobProcessorService>();
                services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
                services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
                services.AddSingleton<IDatabaseService, DatabaseService>();
            });
}