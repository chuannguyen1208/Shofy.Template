using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.WeatherForecasts.Queries
{
    public record GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecaseDTO>>;

    public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecaseDTO>>
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly IMapper _mapper;

        public GetWeatherForecastsQueryHandler(IWeatherForecastRepository weatherForecastRepository, IMapper mapper)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WeatherForecaseDTO>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _weatherForecastRepository.GetManyAsync();

            var result = _mapper.Map<IEnumerable<WeatherForecaseDTO>>(entities);

            return result;
        }
    }
}
