using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Entities
{
    public interface INoTrackingDbContext
    {
        public IQueryable<WeatherForecast> WeatherForecasts { get; }
    }
}
