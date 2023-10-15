﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shofy.Entities;
using Shofy.UseCases.WeatherForecasts;
using Shofy.UseCases.Files;
using Shofy.Infrastructure.EfCore.Documents;

namespace Shofy.Infrastructure.EfCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureEfCore(
           this IServiceCollection services,
           string dbConnectionString)
        {
            static void sqlServerOptionsAction(SqlServerDbContextOptionsBuilder b) => b
                .MigrationsAssembly("Shofy.Infrastructure.EfCore")
                .CommandTimeout(300);

            static void addSqlServerOptions(DbContextOptionsBuilder options) => options
                .UseLazyLoadingProxies()
                //.UseSqlServer(
                //    connectionString: dbConnectionString,
                //    sqlServerOptionsAction: sqlServerOptionsAction);
                .UseInMemoryDatabase(databaseName: "Shofy");

            services
                //	Using dbContext which pooling, which reuses dbContext instance base on it's state
                .AddDbContextPool<ApplicationDbContext>(
                    optionsAction: addSqlServerOptions,
                    poolSize: 1024)
                .AddDbContext<INoTrackingDbContext, ApplicationDbContext>(
                    optionsAction: options => options
                        .UseSqlServer(
                            connectionString: dbConnectionString,
                            sqlServerOptionsAction: sqlServerOptionsAction)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution),
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Singleton);

            services.AddScoped<IWeatherForecastRepository, WeatherForecastsRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}