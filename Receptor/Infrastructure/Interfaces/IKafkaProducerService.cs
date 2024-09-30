namespace Receptor.Infrastructure.Interfaces;

public interface IKafkaProducerService
{
    Task SendMessage(Guid key, string topic, string message);
}