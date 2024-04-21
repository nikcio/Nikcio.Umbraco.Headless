using Nikcio.UHeadless.Common.Reflection;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.ContentItems;

/// <summary>
/// A repository to get content from Umbraco
/// </summary>
public interface IContentItemRepository<out TContentItem>
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
    private readonly IPublishedSnapshotService _publishedSnapshotService;

    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public ContentItemRepository(IPublishedSnapshotService publishedSnapshotService, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        _publishedSnapshotService = publishedSnapshotService;
        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    public TContentItem? GetContentItem(ContentItemBase.CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return ContentItemBase.CreateContentItem<TContentItem>(command, _dependencyReflectorFactory);
    }

    public IPublishedContentCache? GetCache()
    {
        IPublishedSnapshot publishedSnapshot = _publishedSnapshotService.CreatePublishedSnapshot("IncludePreview");

        return publishedSnapshot.Content;
    }
}
