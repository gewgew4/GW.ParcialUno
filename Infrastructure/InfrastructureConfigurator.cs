﻿using Infrastructure.Kafka;
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
        // Repository
        services.AddDbContext<PContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // DI
        services.AddScoped<IDocumentRepo, DocumentRepo>();
        services.AddScoped<IPrintJobRepo, PrintJobRepo>();
        services.AddScoped<IPrintResultRepo, PrintResultRepo>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Kafka
        services.AddSingleton<IKafkaProducer, KafkaProducer>();

        return services;
    }
}
