﻿namespace Infrastructure.Kafka;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, string message);
}