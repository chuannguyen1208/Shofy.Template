using Microsoft.EntityFrameworkCore;
using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.EfCore
{
    public class ApplicationDbContext : DbContext, INoTrackingDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        private DbSet<WeatherForecast> WeatherForecasts { get; set; }
        IQueryable<WeatherForecast> INoTrackingDbContext.WeatherForecasts => WeatherForecasts;
    }
}
