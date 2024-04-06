using Nikcio.UHeadless.ContentTypes.Commands;
using Nikcio.UHeadless.ContentTypes.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentTypes.Factories;

/// <inheritdoc/>
public class ContentTypeFactory<TContentType> : IContentTypeFactory<TContentType>
    where TContentType : IContentType
{
    /// <summary>
    /// A factory that can create objects with DI
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <inheritdoc/>
    public ContentTypeFactory(IDependencyReflectorFactory dependencyReflectorFactory)
    {
        this.dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <inheritdoc/>
    public virtual TContentType? CreateContentType(IPublishedContentType publishedContentType)
    {
        var createContentTypeCommand = new CreateContentType(publishedContentType);

        IContentType? createdContentType = dependencyReflectorFactory.GetReflectedType<IContentType>(typeof(TContentType), new object[] { createContentTypeCommand });

        return createdContentType == null ? default : (TContentType) createdContentType;
    }
}
