using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Receptor;

public static class KafkaProducer
{
    private static IProducer<string, string> _producer;

    static KafkaProducer()
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["KafkaSettings:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public static async Task SendMessage(Guid key, string topic, string message)
    {
        await _producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = Convert.ToString(key)!,
            Value = message
        });
    }
}