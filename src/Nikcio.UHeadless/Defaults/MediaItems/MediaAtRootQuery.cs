using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.MediaItems;

/// <summary>
/// Implements the <see cref="MediaAtRoot" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MediaAtRootQuery
{
    /// <summary>
    /// Gets all the media items at root level
    /// </summary>
    [GraphQLDescription("Gets all the media items at root level.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<MediaItem?> MediaAtRoot(
        IResolverContext resolverContext,
        [Service] ILogger<MediaAtRootQuery> logger,
        [Service] IMediaItemRepository<MediaItem> mediaItemRepository,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            logger.LogError("Media cache is null");
            return new PaginationResult<MediaItem?>(Enumerable.Empty<MediaItem?>(), page, pageSize);
        }

        IEnumerable<IPublishedContent> mediaItems = mediaCache.GetAtRoot();

        IEnumerable<MediaItem?> resultItems = mediaItems.Select(mediaItem => mediaItemRepository.GetMediaItem(new MediaItemBase.CreateCommand()
        {
            PublishedContent = mediaItem,
            ResolverContext = resolverContext,
        }));

        return new PaginationResult<MediaItem?>(resultItems, page, pageSize);
    }
}
