using Microsoft.EntityFrameworkCore;
using Shofy.Entities;
using Shofy.UseCases.WeatherForecasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.EfCore
{
    internal class WeatherForecastsRepository : IWeatherForecastRepository
    {
        private readonly ApplicationDbContext _context;

        public WeatherForecastsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<WeatherForecast> WeatherForecasts => _context.WeatherForecasts;

        public async Task<WeatherForecast> CreateAsync(WeatherForecast weatherForecast)
        {
            var result = await _context.WeatherForecasts.AddAsync(weatherForecast);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<WeatherForecast>> GetManyAsync()
        {
            return await WeatherForecasts.AsNoTracking().ToListAsync();
        }
    }
}
