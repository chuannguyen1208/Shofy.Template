using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterSortPageBuilder;

public interface IFilterSortPageConfig<TItem, TArgs, TKey> :
  IFilterConfig<TItem, TArgs>,
  ISortConfig<TItem, TKey>,
  IPageConfig
{ }

public static class IFilterSortPageExtensions
{
    public static IQueryable<TItem> FilterSortAndGetPage<TItem, TArgs, TKey>(
        this IQueryable<TItem> items,
        IFilterSortPageConfig<TItem, TArgs, TKey> config,
        TArgs args)
    {
        if (config == null)
        {
            return items;
        }
        return items
            .Filter(config, args)
            .Sort<TItem, IFilterSortPageConfig<TItem, TArgs, TKey>, TKey>(config)
            .GetPage(config);
    }

    public static IQueryable<TItem> FilterSortAndGetPage<TItem, TArgs, TKey>(
        this IQueryable<TItem> items,
        IFilterSortPageConfig<TItem, TArgs, TKey> config,
        TArgs args,
        Action<IQueryable<TItem>> onFiltered)
    {
        if (config == null)
        {
            return items;
        }
        var data = items.Filter(config, args);
        onFiltered(data);
        return data
            .Sort<TItem, IFilterSortPageConfig<TItem, TArgs, TKey>, TKey>(config)
            .GetPage(config);
    }

    public static IQueryable<TItem> FilterSortAndGetPage<TItem, TArgs, TKey>(
        this IQueryable<TItem> items,
        IFilterSortPageConfig<TItem, TArgs, TKey> config,
        TArgs args,
        out int itemCount)
    {
        var data = items.Filter(config, args);
        itemCount = data.Count();
        if (config == null)
        {
            return items;
        }
        return data
            .Sort<TItem, IFilterSortPageConfig<TItem, TArgs, TKey>, TKey>(config)
            .GetPage(config);
    }
}
