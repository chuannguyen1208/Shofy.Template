using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shofy.Entities;
using Shofy.UseCases.WeatherForecasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.MongoDb.WeatherFocasts
{
    public class WeatherForecastsMongoRepository : IWeatherForecastRepository
    {
        private readonly IMongoCollection<WeatherForecast> _forecastsCollection;

        public WeatherForecastsMongoRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _forecastsCollection = mongoDatabase.GetCollection<WeatherForecast>(databaseSettings.Value.WeathersCollectionName);
        }

        public IQueryable<WeatherForecast> WeatherForecasts => _forecastsCollection.AsQueryable();

        public async Task<WeatherForecast> CreateAsync(WeatherForecast weatherForecast)
        {
            await _forecastsCollection.InsertOneAsync(weatherForecast);
            return weatherForecast;
        }

        public async Task<IEnumerable<WeatherForecast>> GetManyAsync()
        {
            return await _forecastsCollection.Find(_ => true).ToListAsync();
        }
    }
}
