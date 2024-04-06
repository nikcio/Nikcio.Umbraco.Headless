using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Base.Elements.Repositories;
using Nikcio.UHeadless.Content.Factories;
using Nikcio.UHeadless.Content.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace Nikcio.UHeadless.Content.Repositories;

/// <inheritdoc/>
public class ContentRepository<TContent> : CachedElementRepository<TContent>, IContentRepository<TContent>
    where TContent : IContent
{
    /// <inheritdoc/>
    public ContentRepository(IPublishedSnapshotAccessor publishedSnapshotAccessor, IUmbracoContextFactory umbracoContextFactory, IContentFactory<TContent> contentFactory, ILogger<ContentRepository<TContent>> logger) : base(publishedSnapshotAccessor, umbracoContextFactory, contentFactory, logger)
    {
        ArgumentNullException.ThrowIfNull(umbracoContextFactory);

        umbracoContextFactory.EnsureUmbracoContext();
    }

    /// <inheritdoc/>
    public virtual TContent? GetContent(Func<IPublishedContentCache?, IPublishedContent?> fetch, string? culture, string? segment, Fallback? fallback)
    {
        return base.GetElement(fetch, culture, segment, fallback, publishedSnapshot => publishedSnapshot.Content);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TContent?> GetContentList(Func<IPublishedContentCache?, IEnumerable<IPublishedContent>?> fetch, string? culture, string? segment, Fallback? fallback)
    {
        return base.GetElementList(fetch, culture, segment, fallback, publishedSnapshot => publishedSnapshot.Content);
    }

    /// <inheritdoc/>
    public virtual TContent? GetConvertedContent(IPublishedContent? content, string? culture, string? segment, Fallback? fallback)
    {
        return base.GetConvertedElement(content, culture, segment, fallback);
    }
}
