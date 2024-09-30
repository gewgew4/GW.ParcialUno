using Confluent.Kafka;

namespace Receptor.Infrastructure.Interfaces;

public interface IKafkaConsumerService
{
    Task ConsumeMessages(Func<ConsumeResult<Ignore, string>, Task> messageHandler, CancellationToken cancellationToken);
}