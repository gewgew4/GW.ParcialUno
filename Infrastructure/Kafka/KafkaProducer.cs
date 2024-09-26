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
            BootstrapServers = kafkaSettings.Value.BootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
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
