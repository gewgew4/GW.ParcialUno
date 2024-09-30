using Dapper;
using System.Data.SqlClient;

namespace Receptor;

public static class DatabaseOperations
{
    private static readonly string _connectionString = "Server=sqlserver,1433;Database=GWParcialUno;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;";

    public static async Task UpdatePrintJobStatus(Guid jobId, int status)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            "UPDATE PrintJobs SET Status = @Status WHERE Id = @JobId",
            new { Status = status, JobId = jobId });
    }
}