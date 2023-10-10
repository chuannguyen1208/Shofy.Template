using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterSortPageBuilder;

public interface IPageConfig
{
    int PageSize { get; }
    int PageNumber { get; }
}

public static class IQueryablePageExtensions
{
    public static IQueryable<TItem> GetPage<TItem, TConfig>(
        this IOrderedQueryable<TItem> items,
        TConfig config)
        where TConfig : IPageConfig
    {
        if (config == null)
        {
            return items;
        }
        if (config.PageSize == 0)
        {
            return Enumerable
                .Empty<TItem>()
                .AsQueryable();
        }
        var skip = config.PageSize * Math.Max(config.PageNumber, 0);
        return items
            .Skip(skip)
            .Take(config.PageSize);
    }

    public static IQueryable<TItem> GetPage<TItem, TConfig>(
       this IOrderedQueryable<TItem> items,
       TConfig config,
       out int itemCount)
       where TConfig : IPageConfig
    {
        itemCount = items.Count();
        return items.GetPage(config);
    }
}
