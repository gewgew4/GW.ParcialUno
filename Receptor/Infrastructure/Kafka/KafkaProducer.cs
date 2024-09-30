using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Receptor.Infrastructure.Interfaces;

namespace Receptor.Infrastructure.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly ProducerConfig _config;

    public KafkaProducerService(IConfiguration configuration)
    {
        _config = new ProducerConfig
        {
            BootstrapServers = configuration["KafkaSettings:BootstrapServers"]
        };
    }

    public async Task SendMessage(Guid key, string topic, string message)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();
        await producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = key.ToString(),
            Value = message
        });
    }
}