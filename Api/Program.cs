using Api.Middlewares;
using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();


        // Serilog
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        builder.Logging.AddSerilog(logger);

        // Add configurations
        builder.Services.InfrastructureConfigureServices(builder.Configuration);
        builder.Services.ApplicationConfigureServices(builder.Configuration);

        var app = builder.Build();

        // Automigrate database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<PContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var loggerDb = services.GetRequiredService<ILogger<Program>>();
                loggerDb.LogError(ex, "An error occurred while migrating the database.");
            }
        }

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseMiddleware<ExceptionHandlerMiddleware>(app.Environment.IsDevelopment());
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
