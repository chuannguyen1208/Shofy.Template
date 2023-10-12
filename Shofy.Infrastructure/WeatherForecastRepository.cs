using Shofy.Entities;
using Shofy.UseCases.WeatherForecasts;

namespace Shofy.Infrastructure
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly List<WeatherForecast> _forecasts;

        public WeatherForecastRepository()
        {
            _forecasts = new List<WeatherForecast>();
        }

        public IQueryable<WeatherForecast> WeatherForecasts => _forecasts.AsQueryable();

        public Task<WeatherForecast> CreateAsync(WeatherForecast weatherForecast)
        {
            weatherForecast.Id = Guid.NewGuid();

            _forecasts.Add(weatherForecast);
            
            return Task.FromResult(weatherForecast);
        }

        public Task<IEnumerable<WeatherForecast>> GetManyAsync()
        {
            return Task.FromResult(_forecasts.AsEnumerable());
        }
    }
}