using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Shared.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Content.Queries.ContentByContentType;

/// <summary>
/// Implements the <see cref="ContentByContentType" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentByContentTypeQuery
{
    /// <summary>
    /// Gets all the content items by content type
    /// </summary>
    /// <param name="contentItemRepository"></param>
    /// <param name="preview"></param>
    /// <param name="variationContextAccessor"></param>
    /// <param name="logger"></param>
    /// <param name="resolverContext"></param>
    /// <param name="contentType"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets all the content items by content type.")]
    [UsePaging]
    public virtual IEnumerable<ContentItem?> ContentByContentType(
        IResolverContext resolverContext,
        [Service] ILogger<ContentByContentTypeQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [Service] IVariationContextAccessor variationContextAccessor,
        [GraphQLDescription("The contentType to fetch.")] string contentType,
        [GraphQLDescription("The culture.")] string? culture = null,
        [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
        [GraphQLDescription("The property variation segment")] string? segment = null,
        [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

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

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(preview, culture)
                    .SelectMany(content => content.DescendantsOrSelf(variationContextAccessor, culture))
                    .Where(content => content.ContentType.Id == publishedContentType.Id);

        return contentItems.Select(contentItem => contentItemRepository.GetContentItem(new ContentItemBase.CreateCommand()
        {
            Culture = culture,
            Fallback = fallback?.ToFallback(),
            IsPreview = preview,
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Segment = segment
        }));
    }
}
