using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FilterSortPageBuilder;

public interface IOrderByHandler<TItem>
{
    IOrderByHandler<TItem> SetNext(IOrderByHandler<TItem> handler);
    IOrderedQueryable<TItem>? Order(IOrderedQueryable<TItem>? orderedItems, string orderBy, bool orderDesc, IQueryable<TItem> originalItems);
}

public abstract class OrderBy<TItem, TValue> : Dictionary<string, Expression<Func<TItem, TValue>>>, IOrderByHandler<TItem>
{
    protected OrderBy()
        : base(new CaseInsensitiveEqualityComparer()) { }

    private IOrderByHandler<TItem>? _nextHandler;

    public IOrderedQueryable<TItem>? Order(IOrderedQueryable<TItem>? orderedItems, string orderBy, bool orderDesc, IQueryable<TItem> originalItems)
    {
        var hasKey = this.TryGetValue(orderBy, out var expression);

        if (hasKey)
        {
            if (orderedItems is null)
            {
                orderedItems = orderDesc ? originalItems.OrderByDescending(expression!) : originalItems.OrderBy(expression!);
            }
            else
            {
                orderedItems = orderDesc ? orderedItems.ThenByDescending(expression!) : orderedItems.ThenBy(expression!);
            }

            return orderedItems;
        }

        return _nextHandler?.Order(orderedItems, orderBy, orderDesc, originalItems);
    }

    public IOrderByHandler<TItem> SetNext(IOrderByHandler<TItem> handler)
    {
        _nextHandler = handler;

        return handler;
    }
}

internal class CaseInsensitiveEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string x, string y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

    public int GetHashCode(string obj) => obj.GetHashCode(StringComparison.OrdinalIgnoreCase);
}

public class OrderByBool<TItem> : OrderBy<TItem, bool> { }
public class OrderByString<TItem> : OrderBy<TItem, string> { }
public class OrderByInt<TItem> : OrderBy<TItem, int?> { }
public class OrderByDateTime<TItem> : OrderBy<TItem, DateTime?> { }
public class OrderByDateTimeOffset<TItem> : OrderBy<TItem, DateTimeOffset?> { }
public class OrderByDecimal<TItem> : OrderBy<TItem, decimal?> { }
public class OrderByDateOnly<TItem> : OrderBy<TItem, DateOnly?> { }
