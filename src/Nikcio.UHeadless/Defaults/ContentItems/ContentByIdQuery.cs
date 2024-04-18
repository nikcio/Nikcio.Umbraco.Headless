using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Content.Queries.ContentByRoute;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentById" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentByIdQuery
{
    /// <summary>
    /// Gets a content item by id
    /// </summary>
    [GraphQLDescription("Gets a content item by id.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public ContentItem? ContentById(
        IResolverContext resolverContext,
        [Service] ILogger<ContentByIdQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [GraphQLDescription("The id to fetch.")] int id)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        if (id <= 0)
        {
            throw new ArgumentException("Id must be greater than 0", nameof(id));
        }

        bool includePreview = resolverContext.GetOrSetGlobalState(ContextDataKeys.IncludePreview, _ => false);

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            logger.LogError("Content cache is null");
            return default;
        }

        IPublishedContent? contentItem = contentCache.GetById(includePreview, id);

        return contentItemRepository.GetContentItem(new ContentItemBase.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
        });
    }
}
