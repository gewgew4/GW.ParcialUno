using Common.Messages;
using Confluent.Kafka;
using Dapper;
using Domain;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text.Json;

namespace PrintStatusConsumer;

internal class Program
{
    private static string _connectionString = string.Empty;
    private static string _kafkaBootstrapServers = string.Empty;
    private static string _topicName = string.Empty;
    private static string _consumerName = string.Empty;

    static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _kafkaBootstrapServers = configuration["KafkaSettings:BootstrapServers"]!;
        _topicName = configuration["KafkaSettings:TopicName"]!;
        _consumerName = configuration["KafkaSettings:ConsumerName"]!;

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaBootstrapServers,
            GroupId = _consumerName,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_topicName);

        // Cancellation token to gracefully stop the consumer
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(cts.Token);

                    var jobId = Guid.Parse(consumeResult.Message.Key);
                    var printStatus = JsonSerializer.Deserialize<PrintStatusMessage>(consumeResult.Message.Value);

                    await ProcessPrintStatus(jobId, printStatus!);

                    Console.WriteLine($"Processed print status for JobId: {jobId}");
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }

    static async Task ProcessPrintStatus(Guid jobId, PrintStatusMessage printStatus)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        try
        {
            // Find DocumentId based on DocumentName
            var documentId = await connection.ExecuteScalarAsync<Guid>(
                "SELECT Id FROM Documents WHERE Name = @DocumentName",
                new { printStatus.DocumentName },
                transaction
            );

            if (documentId == Guid.Empty)
            {
                throw new Exception($"Document with name {printStatus.DocumentName} not found.");
            }

            // Update Documents table
            await connection.ExecuteAsync(
                "UPDATE Documents SET Status = 1 WHERE Id = @DocumentId",
                new { DocumentId = documentId },
                transaction
            );

            // Update PrintJobs table
            await connection.ExecuteAsync(
                "UPDATE PrintJobs SET CompletedAt = @CompletedAt, Status = 3 WHERE Id = @JobId",
                new { CompletedAt = printStatus.PrintDate, JobId = jobId },
                transaction
            );

            // Get CreatedAt from PrintJobs table
            var createdAt = await connection.ExecuteScalarAsync<DateTime>(
                "SELECT CreatedAt FROM PrintJobs WHERE Id = @JobId",
                new { JobId = jobId },
                transaction
            );

            // Insert into PrintResults table
            var printResult = new PrintResult()
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                DocumentName = printStatus.DocumentName,
                PrintedAt = printStatus.PrintDate,
                RecordedAt = createdAt
            };

            await connection.ExecuteAsync(
                @"INSERT INTO PrintResults (Id, DocumentId, DocumentName, PrintedAt, RecordedAt) 
                      VALUES (@Id, @DocumentId, @DocumentName, @PrintedAt, @RecordedAt)",
                printResult,
                transaction
            );

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error: {ex.Message}");

            throw;
        }
    }
}
