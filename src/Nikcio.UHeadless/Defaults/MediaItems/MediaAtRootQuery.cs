using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
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
    [UsePaging]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<MediaItem?> MediaAtRoot(
        IResolverContext resolverContext,
        [Service] ILogger<MediaAtRootQuery> logger,
        [Service] IMediaItemRepository<MediaItem> mediaItemRepository)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            logger.LogError("Media cache is null");
            return Enumerable.Empty<MediaItem?>();
        }

        IEnumerable<IPublishedContent> mediaItems = mediaCache.GetAtRoot();

        return mediaItems.Select(mediaItem => mediaItemRepository.GetMediaItem(new MemberItem.CreateCommand()
        {
            PublishedContent = mediaItem,
            ResolverContext = resolverContext,
        }));
    }
}
