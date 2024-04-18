using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.MediaItems;

/// <summary>
/// Implements the <see cref="MediaByContentType" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MediaByContentTypeQuery
{
    /// <summary>
    /// Gets all the media items by content type
    /// </summary>
    [GraphQLDescription("Gets all the media items by content type.")]
    [UsePaging]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<MediaItem?> MediaByContentType(
        IResolverContext resolverContext,
        [Service] ILogger<MediaByContentTypeQuery> logger,
        [Service] IMediaItemRepository<MediaItem> mediaItemRepository,
        [GraphQLDescription("The content type to fetch.")] string contentType)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            logger.LogError("Media cache is null");
            return Enumerable.Empty<MediaItem?>();
        }

        IPublishedContentType? mediaContentType = mediaCache.GetContentType(contentType);

        if (mediaContentType == null)
        {
            logger.LogError("Media type not found. {ContentType}", contentType);
            return Enumerable.Empty<MediaItem?>();
        }

        IEnumerable<IPublishedContent> mediaItems = mediaCache.GetByContentType(mediaContentType);

        return mediaItems.Select(mediaItem => mediaItemRepository.GetMediaItem(new MediaItemBase.CreateCommand()
        {
            PublishedContent = mediaItem,
            ResolverContext = resolverContext,
        }));
    }
}
