using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shofy.UseCases.WeatherForecasts;
using Shofy.UseCases.WeatherForecasts.Commands;
using Shofy.UseCases.WeatherForecasts.Queries;

namespace Shofy.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecaseDTO>> Get()
        {
            var result = await _mediator.Send(new GetWeatherForecastsQuery());
            return result;
        }

        [HttpPost]
        public async Task<WeatherForecaseDTO> Create([FromBody] CreateWeatherForecastCommand command)
        {
            var res = await _mediator.Send(command);
            return res;
        }
    }
}