using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Base.Elements.Commands;
using Nikcio.UHeadless.Base.Elements.Models;
using Nikcio.UHeadless.Base.Elements.Repositories;
using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Factories;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace Nikcio.UHeadless.Content.Repositories;

/// <inheritdoc/>
public class ContentRepository<TContent> : CachedElementRepository<TContent>, IContentRepository<TContent>
    where TContent : class, IContent
{
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    private readonly IContentFactory<TContent> _contentFactory;

    public ContentRepository(IPublishedSnapshotAccessor publishedSnapshotAccessor, IContentFactory<TContent> contentFactory)
    {
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
        _contentFactory = contentFactory;
    }

    /// <inheritdoc/>
    public virtual TContent? GetContent(Func<IPublishedContentCache?, IPublishedContent?> fetch, string? culture, string? segment, Fallback? fallback)
    {
        ArgumentNullException.ThrowIfNull(fetch);

        if (!_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot) || publishedSnapshot == null)
        {
            logger.LogError("Unable to get publishedSnapShot");
            return default;
        }

        IPublishedContentCache? cache = publishedSnapshot.Content;

        IPublishedContent? contentItem = fetch(cache);
        return _contentFactory.CreateContent(contentItem, culture, segment, fallback);
    }

    /// <inheritdoc/>
    [Obsolete("Use GetContentItems instead")]
    public virtual IEnumerable<TContent?> GetContentList(Func<IPublishedContentCache?, IEnumerable<IPublishedContent>?> fetch, string? culture, string? segment, Fallback? fallback)
    {
        return GetContentItems(fetch, culture, segment, fallback);
    }

    public virtual IEnumerable<TContent?> GetContentItems(Func<IPublishedContentCache?, IEnumerable<IPublishedContent>?> fetch, string? culture, string? segment, Fallback? fallback)
    {
        ArgumentNullException.ThrowIfNull(fetch);

        if (!_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot) || publishedSnapshot == null)
        {
            logger.LogError("Unable to get publishedSnapShot");
            return Enumerable.Empty<TContent?>();
        }

        IPublishedContentCache? cache = publishedSnapshot.Content;

        IEnumerable<IPublishedContent>? contentItems = fetch(cache);

        if (contentItems == null)
        {
            logger.LogWarning("Fetching content items resulted in null");
            return Enumerable.Empty<TContent?>();
        }

        return contentItems.Select(contentItem => _contentFactory.CreateContent(contentItem, culture, segment, fallback));
    }

    /// <inheritdoc/>
    public virtual TContent? GetConvertedContent(IPublishedContent? content, string? culture, string? segment, Fallback? fallback)
    {
        return base.GetConvertedElement(content, culture, segment, fallback);
    }
}

public interface IContentFactory<TContent>
    where TContent : class, IContent
{
    /// <summary>
    /// Creates a content document model
    /// </summary>
    /// <param name="content"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    TContent? CreateContent(IPublishedContent? content, string? culture, string? segment, Fallback? fallback);
}


/// <inheritdoc/>
public class ContentFactory<TContent> : IContentFactory<TContent>
    where TContent : class, IContent
{
    /// <summary>
    /// A factory that can create object with DI
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <summary>
    /// The published value fallback
    /// </summary>
    protected IPublishedValueFallback publishedValueFallback { get; }

    /// <inheritdoc/>
    public ContentFactory(IDependencyReflectorFactory dependencyReflectorFactory, IPublishedValueFallback publishedValueFallback)
    {
        this.dependencyReflectorFactory = dependencyReflectorFactory;
        this.publishedValueFallback = publishedValueFallback;
    }

    /// <inheritdoc/>
    public virtual TContent? CreateContent(IPublishedContent? content, string? culture, string? segment, Fallback? fallback)
    {
        var createElementCommand = new CreateElement(content, culture, segment, fallback);
        var createContentCommand = new CreateContent(content, culture, createElementCommand);

        TContent? createdContent = dependencyReflectorFactory.GetReflectedType<TContent>(typeof(TContent), new object[] { createContentCommand });

        if (createdContent == null)
        {
            return default;
        }
        else
        {
            return createdContent;
        }
    }
}
