using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.ContentItems;
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
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public Pagination<ContentItem?> ContentAtRoot(
        IResolverContext resolverContext,
        [Service] ILogger<ContentAtRootQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        bool includePreview = resolverContext.GetOrSetGlobalState(ContextDataKeys.IncludePreview, _ => false);
        string? culture = resolverContext.GetOrSetGlobalState<string?>(ContextDataKeys.Culture, _ => null);

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            logger.LogError("Content cache is null");
            return new Pagination<ContentItem?>(Enumerable.Empty<ContentItem?>(), page, pageSize);
        }

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(includePreview, culture);

        IEnumerable<ContentItem?> resultItems = contentItems.Select(contentItem => contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        }));

        return new Pagination<ContentItem?>(resultItems, page, pageSize);
    }
}
