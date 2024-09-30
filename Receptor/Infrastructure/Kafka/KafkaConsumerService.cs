using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Receptor.Infrastructure.Interfaces;

namespace Receptor.Infrastructure.Kafka;

public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly ConsumerConfig _config;
    private readonly string _topic;

    public KafkaConsumerService(IConfiguration configuration)
    {
        _config = new ConsumerConfig
        {
            BootstrapServers = configuration["KafkaSettings:BootstrapServers"],
            GroupId = configuration["KafkaSettings:ConsumerName"],
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        _topic = configuration["KafkaSettings:TopicNameJobs"]!;
    }

    public async Task ConsumeMessages(Func<ConsumeResult<Ignore, string>, Task> messageHandler, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));
                    if (consumeResult != null)
                    {
                        await messageHandler(consumeResult);
                        consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }
}