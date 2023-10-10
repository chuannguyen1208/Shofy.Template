using AutoMapper;
using MediatR;
using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.WeatherForecasts.Commands
{
    public record CreateWeatherForecastCommand(DateOnly Date, int TemperatureC, string? Summary) : IRequest<WeatherForecaseDTO>;

    public record CreateWeaatherForecaseCommandHandler : IRequestHandler<CreateWeatherForecastCommand, WeatherForecaseDTO>
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly IMapper _mapper;

        public CreateWeaatherForecaseCommandHandler(
            IWeatherForecastRepository weatherForecastRepository, 
            IMapper mapper)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _mapper = mapper;
        }

        public async Task<WeatherForecaseDTO> Handle(CreateWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            WeatherForecast entity = await _weatherForecastRepository.CreateAsync(new WeatherForecast
            {
                Date = request.Date,
                TemperatureC = request.TemperatureC,
                Summary = request.Summary
            });

            var result = _mapper.Map<WeatherForecaseDTO>(entity);

            return result;
        }
    }
}
