using FilterSortPageBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shofy.Entities;
using Shofy.Web.Models.Inputs;
using Shofy.Web.Models.Outputs;

namespace Shofy.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentifiersController : ControllerBase
    {
        private readonly INoTrackingDbContext _noTrackingDbContext;

        public IdentifiersController(INoTrackingDbContext noTrackingDbContext)
        {
            _noTrackingDbContext = noTrackingDbContext;
        }

        [HttpGet("weatherFocasts")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(PagedResults<WeatherForecast>))]
        public IActionResult GetWeatherFocasts([FromQuery] WeatherFocastsGet input)
            => Ok(new PagedResults<WeatherForecast>
            {
                Results = _noTrackingDbContext
                    .WeatherForecasts
                    .FilterSortAndGetPage(
                        config: input.AsFilterSortPageConfig(),
                        args: input,
                        itemCount: out int itemCount),
                ResultCount = itemCount
            });
    }
}
