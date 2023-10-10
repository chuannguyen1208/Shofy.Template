using AutoMapper;
using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.WeatherForecasts
{
    public class WeatherForecastProfile : Profile
    {
        public WeatherForecastProfile()
        {
            CreateMap<WeatherForecast, WeatherForecaseDTO>().ReverseMap();
        }
    }
}
