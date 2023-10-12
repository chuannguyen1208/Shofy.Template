using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.WeatherForecasts
{
    public interface IWeatherForecastRepository
    {
        IQueryable<WeatherForecast> WeatherForecasts { get; }
        Task<WeatherForecast> CreateAsync(WeatherForecast weatherForecast);
        Task<IEnumerable<WeatherForecast>> GetManyAsync();
    }
}
