using Dapper;
using Microsoft.Extensions.Configuration;
using Receptor.Infrastructure.Interfaces;
using System.Data.SqlClient;

namespace Receptor.Infrastructure.Database;

public class DatabaseService(IConfiguration configuration) : IDatabaseService
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task UpdatePrintJobStatus(Guid jobId, int status)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            "UPDATE PrintJobs SET Status = @Status WHERE Id = @JobId",
            new { Status = status, JobId = jobId });
    }
}