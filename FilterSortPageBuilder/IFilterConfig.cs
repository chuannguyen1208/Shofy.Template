using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FilterSortPageBuilder;

public interface IFilterConfig<TItem, TArgs>
{
    FilterPredicate<TItem, TArgs>[] FilterPredicates { get; }
}

public class FilterPredicate<TItem, TArgs>
{
    public readonly Func<TArgs, bool> IsApplicable;
    public readonly Expression<Func<TItem, TArgs, bool>> Predicate;

    public FilterPredicate(
        Func<TArgs, bool> isApplicable,
        Expression<Func<TItem, TArgs, bool>> predicate)
    {
        IsApplicable = isApplicable;
        Predicate = predicate;
    }
}

public static class IQueryableFilterExtensions
{
    public static IQueryable<TItem> Filter<TItem, TConfig, TArgs>(
        this IQueryable<TItem> items,
        TConfig config,
        TArgs args)
        where TConfig : IFilterConfig<TItem, TArgs>
    {
        if (config == null)
        {
            return items;
        }
        items = items
            .AsExpandable();
        foreach (var filterPredicate in config.FilterPredicates.Where(fp => fp.IsApplicable(args)))
        {
            items = items
                .Where(e => filterPredicate.Predicate.Invoke(e, args));
        }
        return items;
    }
}
