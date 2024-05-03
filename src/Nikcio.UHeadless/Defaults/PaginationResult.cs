namespace Nikcio.UHeadless.Defaults;

/// <summary>
/// Represents a paginated list of items
/// </summary>
[GraphQLName("Pagination")]
[GraphQLDescription("Represents a paginated list of items.")]
public class PaginationResult<T>
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "The dual iteation is a count which won't shouldn't decrease performance because we don't need to resolve every content item")]
    public PaginationResult(IEnumerable<T> items, int page, int pageSize)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than or equal to 1");
        }

        if (pageSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 0");
        }

        Page = page;
        PageSize = pageSize;
        TotalItems = items.Count();

        int skip = Page == 0 ? 0 : (Page - 1) * PageSize;
        Items = items.Skip(skip).Take(PageSize).ToList();
    }

    [GraphQLDescription("The page number.")]
    public int Page { get; }

    [GraphQLDescription("The page size")]
    public int PageSize { get; }

    [GraphQLDescription("The total number of items.")]
    public int TotalItems { get; }

    [GraphQLDescription("Whether there is a next page.")]
    public bool HasNextPage => Page * PageSize < TotalItems;

    [GraphQLDescription("The items in the paginated list.")]
    public virtual List<T> Items { get; }
}
