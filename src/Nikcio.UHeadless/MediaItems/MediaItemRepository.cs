using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.MediaItems;

/// <summary>
/// A repository to get media from Umbraco
/// </summary>
public interface IMediaItemRepository<out TMediaItem>
    where TMediaItem : MediaItemBase
{
    /// <summary>
    /// Gets the media model based on the media item from Umbraco
    /// </summary>
    /// <param name="command">The create command for a media item.</param>
    /// <returns></returns>
    TMediaItem? GetMediaItem(MediaItemBase.CreateCommand command);

    /// <summary>
    /// Gets the media cache.
    /// </summary>
    /// <returns></returns>
    IPublishedMediaCache? GetCache();
}

internal class MediaItemRepository<TMediaItem> : IMediaItemRepository<TMediaItem>
    where TMediaItem : MediaItemBase
{
    private readonly IPublishedMediaCache _publishedMediaCache;

    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public MediaItemRepository(IPublishedMediaCache publishedMediaCache, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        _publishedMediaCache = publishedMediaCache;
        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    public TMediaItem? GetMediaItem(MediaItemBase.CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return MediaItemBase.CreateMediaItem<TMediaItem>(command, _dependencyReflectorFactory);
    }

    public IPublishedMediaCache? GetCache()
    {
        return _publishedMediaCache;
    }
}
