using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common.Reflection;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.ContentItems;

/// <summary>
/// A repository to get content from Umbraco
/// </summary>
public interface IContentItemRepository<TContentItem>
    where TContentItem : ContentItemBase
{
    /// <summary>
    /// Gets the content model based on the content item from Umbraco
    /// </summary>
    /// <param name="command">The create command for a content item.</param>
    /// <returns></returns>
    TContentItem? GetContentItem(ContentItemBase.CreateCommand command);

    /// <summary>
    /// Gets the content cache.
    /// </summary>
    /// <returns></returns>
    IPublishedContentCache? GetCache();
}

internal class ContentItemRepository<TContentItem> : IContentItemRepository<TContentItem>
    where TContentItem : ContentItemBase
{
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    private readonly ILogger<ContentItemRepository<TContentItem>> _logger;

    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public ContentItemRepository(IPublishedSnapshotAccessor publishedSnapshotAccessor, ILogger<ContentItemRepository<TContentItem>> logger, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
        _logger = logger;
        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    public TContentItem? GetContentItem(ContentItemBase.CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return ContentItemBase.CreateContentItem<TContentItem>(command, _dependencyReflectorFactory);
    }

    public IPublishedContentCache? GetCache()
    {
        if (!_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot) || publishedSnapshot == null)
        {
            _logger.LogError("Unable to get publishedSnapShot");
            return default;
        }

        return publishedSnapshot.Content;
    }
}
