using Infrastructure.Repo;
using Infrastructure.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureConfigurator
{
    public static IServiceCollection InfrastructureConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<PContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IDocumentRepo, DocumentRepo>();
        services.AddScoped<IPrintJobRepo, PrintJobRepo>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
