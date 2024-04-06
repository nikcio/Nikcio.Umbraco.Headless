using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;

namespace Nikcio.UHeadless.Content.Factories;

/// <inheritdoc/>
public class ContentRedirectFactory<TContentRedirect> : IContentRedirectFactory<TContentRedirect>
    where TContentRedirect : IContentRedirect
{
    /// <summary>
    /// A factory for creating models with DI and reflection
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <inheritdoc/>
    public ContentRedirectFactory(IDependencyReflectorFactory dependencyReflectorFactory)
    {
        this.dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <inheritdoc/>
    public virtual TContentRedirect? CreateContentRedirect(CreateContentRedirect createContentRedirectCommand)
    {
        IContentRedirect? createdContent = dependencyReflectorFactory.GetReflectedType<IContentRedirect>(typeof(TContentRedirect), new object[] { createContentRedirectCommand });
        return createdContent == null ? default : (TContentRedirect) createdContent;
    }
}
