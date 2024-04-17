using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Content.Queries.ContentByRoute;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentAtRoot" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentAtRootQuery
{
    /// <summary>
    /// Gets all the content items at root level
    /// </summary>
    [GraphQLDescription("Gets all the content items at root level.")]
    [UsePaging]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<ContentItem?> ContentAtRoot(
        IResolverContext resolverContext,
        [Service] ILogger<ContentAtRootQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        bool includePreview = resolverContext.GetOrSetGlobalState(ContextDataKeys.IncludePreview, _ => false);
        string? culture = resolverContext.GetOrSetGlobalState<string?>(ContextDataKeys.Culture, _ => null);

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            logger.LogError("Content cache is null");
            return Enumerable.Empty<ContentItem?>();
        }

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(includePreview, culture);

        return contentItems.Select(contentItem => contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        }));
    }
}
