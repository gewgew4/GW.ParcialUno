using Common.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Receptor;

internal class Program
{
    private static PriorityQueue<ConsumeResult<Ignore, string>, int> _priorityQueue = new();
    // Process with BatchSize messages or every ProcessInterval
    private static readonly int BatchSize = 10;
    private static readonly TimeSpan ProcessInterval = TimeSpan.FromSeconds(5);
    private static DateTime _lastProcessTime = DateTime.MinValue;

    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["KafkaSettings:BootstrapServers"],
            GroupId = "print-job-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            await RunConsumer(consumerConfig, cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Normal exit
            Console.WriteLine("Exit");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static async Task RunConsumer(ConsumerConfig config, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("print-jobs");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));
                    if (consumeResult != null)
                    {
                        var priority = GetPriority(consumeResult);
                        _priorityQueue.Enqueue(consumeResult, -priority);
                    }

                    if (ShouldProcessQueue())
                    {
                        await ProcessQueue();
                        _lastProcessTime = DateTime.Now;
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error consuming message: {ex.Message}");
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }

    static bool ShouldProcessQueue()
    {
        return _priorityQueue.Count >= BatchSize || (DateTime.Now - _lastProcessTime) >= ProcessInterval;
    }

    static int GetPriority(ConsumeResult<Ignore, string> consumeResult)
    {
        if (consumeResult.Message.Headers.TryGetLastBytes("priority", out var priorityBytes) &&
            priorityBytes != null)
        {
            var priority = Encoding.UTF8.GetString(priorityBytes);
            return Convert.ToInt16(priority);
        }
        return 0;
    }

    static async Task ProcessQueue()
    {
        Console.WriteLine($"Processing queue with {_priorityQueue.Count} messages");
        while (_priorityQueue.TryDequeue(out var result, out var priority))
        {
            await ProcessMessage(result, -priority);
        }
    }

    static async Task ProcessMessage(ConsumeResult<Ignore, string> result, int priority)
    {
        Console.WriteLine($"Processing message with priority {priority}: {result.Message.Value}");

        var printJobMessage = JsonSerializer.Deserialize<PrintJobMessage>(result.Message.Value);

        // Queued
        await DatabaseOperations.UpdatePrintJobStatus(printJobMessage.JobId, 1);

        await Task.Delay(1000);
        var rand = new Random();
        if (rand.Next(0, 2) == 0)
        {
            // Failed
            await DatabaseOperations.UpdatePrintJobStatus(printJobMessage.JobId, 4);
        }
        else
        {
            // Processing
            await DatabaseOperations.UpdatePrintJobStatus(printJobMessage.JobId, 2);

            // Simulated printing
            var printed = DateTime.UtcNow;
            Console.WriteLine(Convert.ToString(printJobMessage.Content));
            await Task.Delay(5000);

            // Send message to Kafka
            var kafkaMessage = JsonSerializer.Serialize(new PrintStatusMessage
            {
                OK = "OK",
                PrintDate = printed,
                DocumentName = printJobMessage.DocumentName
            });

            await KafkaProducer.SendMessage(printJobMessage.JobId, "print-status", kafkaMessage);
        }

        await Task.Delay(1000);
    }
}