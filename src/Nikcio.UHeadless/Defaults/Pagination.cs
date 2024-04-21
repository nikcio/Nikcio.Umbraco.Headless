namespace Nikcio.UHeadless.Defaults;

/// <summary>
/// Represents a paginated list of items
/// </summary>
[GraphQLDescription("Represents a paginated list of items.")]
public class Pagination<T>
{
    protected IEnumerable<T> SourceItems { get; }

    public Pagination(IEnumerable<T> items, int page, int pageSize)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than or equal to 1");
        }

        if (pageSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 0");
        }

        SourceItems = items;
        Page = page;
        PageSize = pageSize;
    }

    [GraphQLDescription("The page number.")]
    public int Page { get; }

    [GraphQLDescription("The page size")]
    public int PageSize { get; }

    [GraphQLDescription("The total number of items.")]
    public int TotalItems => SourceItems.Count();

    [GraphQLDescription("Whether there is a next page.")]
    public bool HasNextPage => Page * PageSize < TotalItems;

    [GraphQLDescription("The items in the paginated list.")]
    public virtual IEnumerable<T> Items()
    {
        int skip = Page == 0 ? 0 : (Page - 1) * PageSize;
        return SourceItems.Skip(skip).Take(PageSize);
    }
}
