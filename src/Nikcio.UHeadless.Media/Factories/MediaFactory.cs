using Nikcio.UHeadless.Base.Elements.Commands;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Nikcio.UHeadless.Media.Commands;
using Nikcio.UHeadless.Media.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Media.Factories;

/// <inheritdoc/>
public class MediaFactory<TMedia> : IMediaFactory<TMedia>
        where TMedia : IMedia
{
    /// <summary>
    /// A factory for creating object with DI
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <inheritdoc/>
    public MediaFactory(IDependencyReflectorFactory dependencyReflectorFactory)
    {
        this.dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <inheritdoc/>
    public TMedia? CreateElement(IPublishedContent? element, string? culture, string? segment, Fallback? fallback)
    {
        var createElementCommand = new CreateElement(element, culture, segment, fallback);
        var createMediaCommand = new CreateMedia(element, createElementCommand);

        IMedia? createdContent = dependencyReflectorFactory.GetReflectedType<IMedia>(typeof(TMedia), new object[] { createMediaCommand });
        return createdContent == null ? default : (TMedia) createdContent;
    }

    /// <inheritdoc/>
    public virtual TMedia? CreateMedia(IPublishedContent media)
    {
        return CreateElement(media, null, null, null);
    }
}
