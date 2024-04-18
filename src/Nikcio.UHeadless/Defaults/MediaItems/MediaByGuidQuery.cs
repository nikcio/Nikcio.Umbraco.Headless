using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.MediaItems;

/// <summary>
/// Implements the <see cref="MediaByGuid" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MediaByGuidQuery
{
    /// <summary>
    /// Gets a Media item by Guid
    /// </summary>
    [GraphQLDescription("Gets a Media item by Guid.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MediaItem? MediaByGuid(
        IResolverContext resolverContext,
        [Service] ILogger<MediaByGuidQuery> logger,
        [Service] IMediaItemRepository<MediaItem> mediaItemRepository,
        [GraphQLDescription("The Guid to fetch.")] Guid id)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            logger.LogError("Media cache is null");
            return default;
        }

        IPublishedContent? mediaItem = mediaCache.GetById(id);

        return mediaItemRepository.GetMediaItem(new MediaItemBase.CreateCommand()
        {
            PublishedContent = mediaItem,
            ResolverContext = resolverContext,
        });
    }
}
