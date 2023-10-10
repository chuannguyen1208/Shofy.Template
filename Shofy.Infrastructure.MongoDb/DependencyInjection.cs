using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shofy.Infrastructure.MongoDb.WeatherFocasts;
using Shofy.UseCases.WeatherForecasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.MongoDb
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("MongoDatabase"));
            services.AddSingleton<IWeatherForecastRepository, WeatherForecastsMongoRepository>();
            return services;
        }
    }
}
