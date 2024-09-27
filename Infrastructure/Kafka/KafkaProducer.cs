using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka;

public class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(ILogger<KafkaProducer> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("BootstrapServers") //kafkaSettings.Value.BootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }

    public async Task<bool> ProduceAsync(string topic, string message)
    {
        // TODO: sacar
        try
        {
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            _logger.LogInformation($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            return false;
        }
        return true;
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}
