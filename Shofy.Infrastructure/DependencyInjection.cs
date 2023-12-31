﻿using Microsoft.Extensions.DependencyInjection;
using Shofy.Entities;
using Shofy.UseCases.Files;
using Shofy.UseCases.WeatherForecasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();
            services.AddSingleton<IDocumentRepository, DocumentRepositoy>();
            services.AddScoped<INoTrackingDbContext, DefaultAppDbContext>();

            return services;
        }
    }
}
