using Application.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationConfigurator
{
    public static IServiceCollection ApplicationConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateDocumentCommand).Assembly);
        });

        // Services
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
