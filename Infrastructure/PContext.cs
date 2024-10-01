using Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructure;

public class PContext(DbContextOptions<PContext> options) : DbContext(options)
{
    public DbSet<Document> Documents { get; set; }
    public DbSet<PrintJob> PrintJobs { get; set; }
    public DbSet<PrintResult> PrintResults { get; set; }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public void Migrate()
    {
        base.Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Props config

        // Seed
        Seed(modelBuilder);
    }

    private void Seed(ModelBuilder modelBuilder)
    {

    }
}
