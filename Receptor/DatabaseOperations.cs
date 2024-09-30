using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Receptor;

public static class DatabaseOperations
{
    private static readonly string _connectionString;

    static DatabaseOperations()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        _connectionString = configuration["ConnectionStrings:DefaultConnection"]
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public static async Task UpdatePrintJobStatus(Guid jobId, int status)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            "UPDATE PrintJobs SET Status = @Status WHERE Id = @JobId",
            new
            {
                Status = status,
                JobId = jobId
            });
    }
}