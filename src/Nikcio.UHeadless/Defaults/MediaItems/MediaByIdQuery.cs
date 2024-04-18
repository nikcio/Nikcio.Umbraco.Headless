using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.MediaItems;

/// <summary>
/// Implements the <see cref="MediaById" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MediaByIdQuery
{
    /// <summary>
    /// Gets a Media item by id
    /// </summary>
    [GraphQLDescription("Gets a Media item by id.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MediaItem? MediaById(
        IResolverContext resolverContext,
        [Service] ILogger<MediaByIdQuery> logger,
        [Service] IMediaItemRepository<MediaItem> mediaItemRepository,
        [GraphQLDescription("The id to fetch.")] int id)
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
