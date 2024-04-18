using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Content.Queries.ContentByRoute;
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
    [UsePaging]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<ContentItem?> ContentByContentType(
        IResolverContext resolverContext,
        [Service] ILogger<ContentByContentTypeQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [Service] IVariationContextAccessor variationContextAccessor,
        [GraphQLDescription("The contentType to fetch.")] string contentType)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        bool includePreview = resolverContext.GetOrSetGlobalState(ContextDataKeys.IncludePreview, _ => false);
        string? culture = resolverContext.GetOrSetGlobalState<string?>(ContextDataKeys.Culture, _ => null);

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            logger.LogError("Content cache is null");
            return Enumerable.Empty<ContentItem?>();
        }

        IPublishedContentType? publishedContentType = contentCache.GetContentType(contentType);
        if (publishedContentType == null)
        {
            logger.LogError("Content type not found");
            return Enumerable.Empty<ContentItem?>();
        }

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(includePreview, culture)
                    .SelectMany(content => content.DescendantsOrSelf(variationContextAccessor, culture))
                    .Where(content => content.ContentType.Id == publishedContentType.Id);

        return contentItems.Select(contentItem => contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        }));
    }
}
