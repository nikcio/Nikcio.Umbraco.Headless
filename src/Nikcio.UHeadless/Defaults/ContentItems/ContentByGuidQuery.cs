using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentByGuid" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentByGuidQuery
{
    /// <summary>
    /// Gets a content item by Guid
    /// </summary>
    [GraphQLDescription("Gets a content item by Guid.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public ContentItem? ContentByGuid(
        IResolverContext resolverContext,
        [Service] ILogger<ContentByGuidQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [GraphQLDescription("The id to fetch.")] Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        bool includePreview = resolverContext.GetOrSetGlobalState(ContextDataKeys.IncludePreview, _ => false);

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            logger.LogError("Content cache is null");
            return default;
        }

        IPublishedContent? contentItem = contentCache.GetById(includePreview, id);

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        });
    }
}