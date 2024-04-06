using Nikcio.UHeadless.Base.Elements.Commands;
using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Content.Factories;

/// <inheritdoc/>
public class ContentFactory<TContent> : IContentFactory<TContent>
        where TContent : IContent
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
        return CreateElement(content, culture, segment, fallback);
    }

    /// <inheritdoc/>
    public TContent? CreateElement(IPublishedContent? element, string? culture, string? segment, Fallback? fallback)
    {
        var createElementCommand = new CreateElement(element, culture, segment, fallback);
        var createContentCommand = new CreateContent(element, culture, createElementCommand);

        IContent? createdContent = dependencyReflectorFactory.GetReflectedType<IContent>(typeof(TContent), new object[] { createContentCommand });
        return createdContent == null ? default : (TContent) createdContent;
    }
}
