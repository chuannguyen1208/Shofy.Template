using Shofy.Entities;
using Shofy.Infrastructure.MongoDb.WeatherFocasts;
using Shofy.UseCases.WeatherForecasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.MongoDb
{
    public class MongoDbContext : INoTrackingDbContext
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        public MongoDbContext(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public IQueryable<WeatherForecast> WeatherForecasts => _weatherForecastRepository.WeatherForecasts;
    }
}
