using Shofy.Entities;
using Shofy.UseCases.WeatherForecasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure
{
    public class DefaultAppDbContext : INoTrackingDbContext
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        public DefaultAppDbContext(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public IQueryable<WeatherForecast> WeatherForecasts => _weatherForecastRepository.WeatherForecasts;
    }
}
