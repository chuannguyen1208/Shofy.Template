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
        return items
            .SortBy<TItem, TConfig, TKey>(config)
            .ThenBy(config.PrimaryKeySelector);
    }

    private static IOrderedQueryable<TItem> SortBy<TItem, TConfig, TKey>(
        this IQueryable<TItem> items,
        TConfig config)
        where TConfig : ISortConfig<TItem, TKey>
    {
        var results = items.OrderBy(e => false);
        foreach (var sorting in config.SortCriteria)
        {
            var parts = sorting.Split('|');
            var orderBy = parts[0];
            var orderDesc = parts[1].ToLower() == "desc";
            if (config.OrderByStringKeySelectors.TryGetValue(orderBy, out var orderByStringKeySelector))
            {
                results = orderDesc ? results.ThenByDescending(orderByStringKeySelector) : results.ThenBy(orderByStringKeySelector);
                continue;
            }

            if (config.OrderByIntKeySelectors.TryGetValue(orderBy, out var orderByIntKeySelector))
            {
                results = orderDesc ? results.ThenByDescending(orderByIntKeySelector) : results.ThenBy(orderByIntKeySelector);
                continue;
            }

            if (config.OrderByDateTimeKeySelectors.TryGetValue(orderBy, out var orderByDateTimeKeySelector))
            {
                results = orderDesc ? results.ThenByDescending(orderByDateTimeKeySelector) : results.ThenBy(orderByDateTimeKeySelector);
                continue;
            }

            if (config.OrderByDateTimeOffsetKeySelectors.TryGetValue(orderBy, out var orderByStringDateTimeOffsetSelector))
            {
                results = orderDesc ? results.ThenByDescending(orderByStringDateTimeOffsetSelector) : results.ThenBy(orderByStringDateTimeOffsetSelector);
                continue;
            }

            if (config.OrderByDecimalKeySelectors.TryGetValue(orderBy, out var orderByDecimalKeySelector))
            {
                results = orderDesc ? results.ThenByDescending(orderByDecimalKeySelector) : results.ThenBy(orderByDecimalKeySelector);
                continue;
            }

            if (config.OrderByBoolKeySelectors.TryGetValue(orderBy, out var orderByBoolKeySelector))
            {
                results = orderDesc ? results.ThenByDescending(orderByBoolKeySelector) : results.ThenBy(orderByBoolKeySelector);
                continue;
            }
        }
        return results;
    }
}
