using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentByTag" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentByTagQuery
{
    /// <summary>
    /// Gets content items by tag
    /// </summary>
    [GraphQLDescription("Gets content items by tag.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<ContentItem?> ContentByTag(
        IResolverContext resolverContext,
        [Service] ILogger<ContentByTagQuery> logger,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [Service] ITagService tagService,
        [GraphQLDescription("The tag to fetch.")] string tag,
        [GraphQLDescription("The tag group to fetch.")] string? tagGroup = null,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentException.ThrowIfNullOrEmpty(tag);
        ArgumentNullException.ThrowIfNull(contentItemRepository);
        ArgumentNullException.ThrowIfNull(tagService);

        string? culture = resolverContext.GetOrSetGlobalState<string?>(ContextDataKeys.Culture, _ => null);

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            logger.LogError("Content cache is null");
            return new PaginationResult<ContentItem?>(Enumerable.Empty<ContentItem?>(), page, pageSize);
        }

        IEnumerable<TaggedEntity> taggedEntities = tagService.GetTaggedContentByTag(tag, tagGroup, culture);
        IEnumerable<IPublishedContent> contentItems = taggedEntities.Select(entity => contentCache?.GetById(entity.EntityId)).OfType<IPublishedContent>();

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
