using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentByContentType" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentByContentTypeQuery
{
    /// <summary>
    /// Gets all the content items by content type
    /// </summary>
    [GraphQLDescription("Gets all the content items by content type.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<ContentItem?> ContentByContentType(
        IResolverContext resolverContext,
        [GraphQLDescription("The contentType to fetch.")] string contentType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();
        IVariationContextAccessor variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContentType? publishedContentType = contentCache.GetContentType(contentType);
        if (publishedContentType == null)
        {
            ILogger<ContentByContentTypeQuery> logger = resolverContext.Service<ILogger<ContentByContentTypeQuery>>();
            logger.LogInformation("Content type not found");
            return new PaginationResult<ContentItem?>(Enumerable.Empty<ContentItem?>(), page, pageSize);
        }

        bool includePreview = resolverContext.IncludePreview();
        string? culture = resolverContext.Culture();

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(includePreview, culture)
                    .SelectMany(content => content.DescendantsOrSelf(variationContextAccessor, culture))
                    .Where(content => content.ContentType.Id == publishedContentType.Id);

        IEnumerable<ContentItem?> resultItems = contentItems.Select(contentItem => contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        }));

        return new PaginationResult<ContentItem?>(resultItems, page, pageSize);
    }
}
