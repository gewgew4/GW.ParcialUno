using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka;


public class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("BootstrapServers") //kafkaSettings.Value.BootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        // TODO: sacar
        try
        {
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            //_logger.($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
            if (true)
            {

            }
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}

public class KafkaSettings
{
    public string BootstrapServers { get; set; }
}
