using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FilterSortPageBuilder;

public interface ISortConfig<TItem, TKey>
{
    IEnumerable<string> SortCriteria { get; }
    OrderByBool<TItem> OrderByBoolKeySelectors { get; }
    OrderByString<TItem> OrderByStringKeySelectors { get; }
    OrderByDateTime<TItem> OrderByDateTimeKeySelectors { get; }
    OrderByDateTimeOffset<TItem> OrderByDateTimeOffsetKeySelectors { get; }
    OrderByInt<TItem> OrderByIntKeySelectors { get; }
    OrderByDecimal<TItem> OrderByDecimalKeySelectors { get; }
    OrderByDateOnly<TItem> OrderByDateOnlySelectors { get; }
    Expression<Func<TItem, TKey>> PrimaryKeySelector { get; }
}

public static class IQueryableSortExtensions
{
    public static string AsAscendingSorter(this string str) => $"{str}|asc";

    public static string AsDescendingSorter(this string str) => $"{str}|desc";

    public static IOrderedQueryable<TItem> Sort<TItem, TConfig, TKey>(
        this IQueryable<TItem> items,
        TConfig config)
        where TConfig : ISortConfig<TItem, TKey>
    {
        if (config == null || config.SortCriteria == null || !config.SortCriteria.Any())
        {
            return Queryable.OrderBy(items, config!.PrimaryKeySelector);
        }

        var res = items.SortBy<TItem, TConfig, TKey>(config);

        return res;
    }

    private static IOrderedQueryable<TItem> SortBy<TItem, TConfig, TKey>(
        this IQueryable<TItem> items,
        TConfig config)
        where TConfig : ISortConfig<TItem, TKey>
    {
        IOrderedQueryable<TItem>? results = null;

        var orderHandler = config.OrderByStringKeySelectors;
        orderHandler
            .SetNext(config.OrderByIntKeySelectors)
            .SetNext(config.OrderByDateTimeKeySelectors)
            .SetNext(config.OrderByDateTimeOffsetKeySelectors)
            .SetNext(config.OrderByDateOnlySelectors)
            .SetNext(config.OrderByDecimalKeySelectors)
            .SetNext(config.OrderByBoolKeySelectors);

        foreach (var sorting in config.SortCriteria)
        {
            var parts = sorting.Split('|');

            var orderBy = parts[0];

            var orderDesc = parts[1].ToLower() == "desc";

            results = orderHandler.Order(
                orderedItems: results,
                orderBy: orderBy,
                orderDesc: orderDesc,
                originalItems: items);
        }

        results = results != null ? results.ThenBy(config.PrimaryKeySelector) : items.OrderBy(config.PrimaryKeySelector);

        return results!;
    }
}
