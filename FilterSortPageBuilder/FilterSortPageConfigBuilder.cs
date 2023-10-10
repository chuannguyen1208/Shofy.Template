using System.Linq.Expressions;

namespace FilterSortPageBuilder;

public class FilterSortPageConfigBuilder<TItem, TArgs, TKey>
{
    private FilterPredicate<TItem, TArgs>[] FilterPredicates;

    private readonly Expression<Func<TItem, TKey>> PrimaryKeySelector;
    private IEnumerable<string> SortCriteria;
    private OrderByBool<TItem>? OrderByBoolKeySelectors;
    private OrderByString<TItem>? OrderByStringKeySelectors;
    private OrderByDateTime<TItem>? OrderByDateTimeKeySelectors;
    private OrderByDateTimeOffset<TItem>? OrderByDateTimeOffsetKeySelectors;
    private OrderByInt<TItem>? OrderByIntKeySelectors;
    private OrderByDecimal<TItem>? OrderByDecimalKeySelectors;

    private readonly int? PageSize;
    private readonly int? PageNumber;

    public FilterSortPageConfigBuilder(
        Expression<Func<TItem, TKey>> primaryKeySelector,
        int? pageSize,
        int? pageNumber)
    {
        FilterPredicates = Array.Empty<FilterPredicate<TItem, TArgs>>();
        SortCriteria = Enumerable.Empty<string>();
        PrimaryKeySelector = primaryKeySelector;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    public FilterSortPageConfigBuilder<TItem, TArgs, TKey> WithFiltering(FilterPredicate<TItem, TArgs>[] filters)
    {
        FilterPredicates = filters;
        return this;
    }

    public FilterSortPageConfigBuilder<TItem, TArgs, TKey> WithSorting(
        IEnumerable<string> sortCriteria,
        OrderByBool<TItem>? orderByBoolKeySelectors = null,
        OrderByString<TItem>? orderByStringKeySelectors = null,
        OrderByDateTime<TItem>? orderByDateTimeKeySelectors = null,
        OrderByDateTimeOffset<TItem>? orderByDateTimeOffsetKeySelectors = null,
        OrderByInt<TItem>? orderByIntKeySelectors = null,
        OrderByDecimal<TItem>? orderByDecimalKeySelectors = null)
    {
        SortCriteria = sortCriteria;
        OrderByBoolKeySelectors = orderByBoolKeySelectors;
        OrderByStringKeySelectors = orderByStringKeySelectors;
        OrderByDateTimeKeySelectors = orderByDateTimeKeySelectors;
        OrderByDateTimeOffsetKeySelectors = orderByDateTimeOffsetKeySelectors;
        OrderByIntKeySelectors = orderByIntKeySelectors;
        OrderByDecimalKeySelectors = orderByDecimalKeySelectors;
        return this;
    }

    public IFilterSortPageConfig<TItem, TArgs, TKey> Build()
        => new FilterSortPage<TItem, TArgs, TKey>(
            filterPredicates: FilterPredicates ?? new FilterPredicate<TItem, TArgs>[0],
            primaryKeySelector: PrimaryKeySelector,
            sortCriteria: SortCriteria ?? new string[] { },
            orderByBoolKeySelectors: OrderByBoolKeySelectors ?? new OrderByBool<TItem>(),
            orderByStringKeySelectors: OrderByStringKeySelectors ?? new OrderByString<TItem>(),
            orderByDateTimeKeySelectors: OrderByDateTimeKeySelectors ?? new OrderByDateTime<TItem>(),
            orderByDateTimeOffsetKeySelectors: OrderByDateTimeOffsetKeySelectors ?? new OrderByDateTimeOffset<TItem>(),
            orderByIntKeySelectors: OrderByIntKeySelectors ?? new OrderByInt<TItem>(),
            orderByDecimalKeySelectors: OrderByDecimalKeySelectors ?? new OrderByDecimal<TItem>(),
            pageSize: PageSize ?? int.MaxValue,
            pageNumber: PageNumber ?? 0);

    private class FilterSortPage<TResultItem, TResultArgs, TResultKey> :
        IFilterSortPageConfig<TResultItem, TResultArgs, TResultKey>
    {
        public FilterPredicate<TResultItem, TResultArgs>[] FilterPredicates { get; private set; }

        public Expression<Func<TResultItem, TResultKey>> PrimaryKeySelector { get; private set; }
        public IEnumerable<string> SortCriteria { get; private set; }
        public OrderByBool<TResultItem> OrderByBoolKeySelectors { get; private set; }
        public OrderByString<TResultItem> OrderByStringKeySelectors { get; private set; }
        public OrderByDateTime<TResultItem> OrderByDateTimeKeySelectors { get; private set; }
        public OrderByDateTimeOffset<TResultItem> OrderByDateTimeOffsetKeySelectors { get; private set; }
        public OrderByInt<TResultItem> OrderByIntKeySelectors { get; private set; }
        public OrderByDecimal<TResultItem> OrderByDecimalKeySelectors { get; private set; }

        public int PageSize { get; }
        public int PageNumber { get; }

        public FilterSortPage(
            FilterPredicate<TResultItem, TResultArgs>[] filterPredicates,
            Expression<Func<TResultItem, TResultKey>> primaryKeySelector,
            IEnumerable<string> sortCriteria,
            OrderByBool<TResultItem> orderByBoolKeySelectors,
            OrderByString<TResultItem> orderByStringKeySelectors,
            OrderByDateTime<TResultItem> orderByDateTimeKeySelectors,
            OrderByDateTimeOffset<TResultItem> orderByDateTimeOffsetKeySelectors,
            OrderByInt<TResultItem> orderByIntKeySelectors,
            OrderByDecimal<TResultItem> orderByDecimalKeySelectors,
            int pageSize,
            int pageNumber)
        {
            FilterPredicates = filterPredicates;
            PrimaryKeySelector = primaryKeySelector;
            SortCriteria = sortCriteria;
            OrderByBoolKeySelectors = orderByBoolKeySelectors;
            OrderByStringKeySelectors = orderByStringKeySelectors;
            OrderByDateTimeKeySelectors = orderByDateTimeKeySelectors;
            OrderByDateTimeOffsetKeySelectors = orderByDateTimeOffsetKeySelectors;
            OrderByIntKeySelectors = orderByIntKeySelectors;
            OrderByDecimalKeySelectors = orderByDecimalKeySelectors;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}