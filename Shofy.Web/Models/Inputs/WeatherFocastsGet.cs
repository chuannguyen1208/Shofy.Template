using FilterSortPageBuilder;
using Shofy.Entities;

namespace Shofy.Web.Models.Inputs
{
    public class WeatherFocastsGet
    {
        public DateOnly? Date { get; set; }
        public string? Summary { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public static class WeatherFocastsGetExtensions
    {
        private static readonly FilterPredicate<WeatherForecast, WeatherFocastsGet>[] FILTERS = new[]
       {
            new FilterPredicate<WeatherForecast, WeatherFocastsGet>(
                isApplicable: input => input.Date.HasValue,
                predicate: (a, input) => a.Date == input.Date)
        };

        private static readonly List<string> DEFAULT_SORT_CRITERIA = new()
        {
            "TemperatureC".AsDescendingSorter(),
            "Date".AsAscendingSorter(),
        };

        private static readonly OrderByString<WeatherForecast>? ORDERBY_STRING_CRITERIA = new()
        {
        };

        private static readonly OrderByInt<WeatherForecast>? ORDERBY_INT_CRITERIA = new()
        {
            //["TemperatureC"] = x => x.TemperatureC
        };

        private static readonly OrderByDateTime<WeatherForecast>? ORDERBY_DATETIME_CRITERIA = new()
        {
        };

        private static readonly OrderByDateOnly<WeatherForecast>? ORDERBY_DATEONLY_CRITERIA = new()
        {
            ["Date"] = x => x.Date,
        };

        public static IFilterSortPageConfig<WeatherForecast, WeatherFocastsGet, Guid> AsFilterSortPageConfig(this WeatherFocastsGet input) =>
            new FilterSortPageConfigBuilder<WeatherForecast, WeatherFocastsGet, Guid>(
                primaryKeySelector: x => x.Id,
                pageSize: input.PageSize,
                pageNumber: input.PageNumber)
            .WithFiltering(FILTERS)
            .WithSorting(
                sortCriteria: DEFAULT_SORT_CRITERIA,
                orderByStringKeySelectors: ORDERBY_STRING_CRITERIA,
                orderByIntKeySelectors: ORDERBY_INT_CRITERIA,
                orderByDateTimeKeySelectors: ORDERBY_DATETIME_CRITERIA,
                orderByDateOnlyKeySelectors: ORDERBY_DATEONLY_CRITERIA)
            .Build();
    }
}
