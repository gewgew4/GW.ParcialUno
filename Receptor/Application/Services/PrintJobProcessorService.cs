using Common.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Receptor.Infrastructure.Interfaces;
using System.Text.Json;

namespace Receptor.Application.Services;

public class PrintJobProcessorService(
    IKafkaConsumerService kafkaConsumerService,
    IKafkaProducerService kafkaProducerService,
    IDatabaseService databaseService,
    IConfiguration configuration) : BackgroundService
{
    private readonly string _statusTopic = configuration["KafkaSettings:TopicNameStatus"];
    private readonly PriorityQueue<PrintJobMessage, int> _priorityQueue = new();
    private readonly int _batchSize = 10;
    private readonly TimeSpan _processInterval = TimeSpan.FromSeconds(10);
    private DateTime _lastProcessTime = DateTime.UtcNow;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await kafkaConsumerService.ConsumeMessages(EnqueueMessage, stoppingToken);
    }

    private Task EnqueueMessage(ConsumeResult<Ignore, string> consumeResult)
    {
        var printJobMessage = JsonSerializer.Deserialize<PrintJobMessage>(consumeResult.Message.Value);
        // Negative for descending order
        _priorityQueue.Enqueue(printJobMessage, -printJobMessage.Priority);

        if (ShouldProcessQueue())
        {
            _lastProcessTime = DateTime.UtcNow;
            // Fire and forget
            _ = ProcessQueueAsync();
        }

        return Task.CompletedTask;
    }

    private bool ShouldProcessQueue()
    {
        return _priorityQueue.Count >= _batchSize || (DateTime.Now - _lastProcessTime) >= _processInterval;
    }

    private async Task ProcessQueueAsync()
    {
        Console.WriteLine($"Processing queue with {_priorityQueue.Count} messages");

        while (_priorityQueue.TryDequeue(out var printJobMessage, out var priority))
        {
            await ProcessPrintJob(printJobMessage);
        }
    }

    private async Task ProcessPrintJob(PrintJobMessage printJob)
    {
        // Queued
        await databaseService.UpdatePrintJobStatus(printJob.JobId, 1);

        await Task.Delay(1000);
        var rand = new Random();
        if (rand.Next(0, 2) == 0)
        {
            // Failed
            await databaseService.UpdatePrintJobStatus(printJob.JobId, 4);
        }
        else
        {
            // Processing
            await databaseService.UpdatePrintJobStatus(printJob.JobId, 2);

            // Simulated printing
            var printed = DateTime.UtcNow;
            Console.WriteLine($"Printing: {printJob.DocumentName}");
            await Task.Delay(5000); 

            // Send message to Kafka
            var printStatusMessage = new PrintStatusMessage
            {
                OK = "OK",
                PrintDate = printed,
                DocumentName = printJob.DocumentName
            };

            await kafkaProducerService.SendMessage(
                printJob.JobId,
                _statusTopic,
                JsonSerializer.Serialize(printStatusMessage)
            );
        }
    }
}