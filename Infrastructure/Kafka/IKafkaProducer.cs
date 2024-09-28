namespace Infrastructure.Kafka;

public interface IKafkaProducer
{
    Task<bool> ProduceAsync(string topic, string message, byte priority);
}