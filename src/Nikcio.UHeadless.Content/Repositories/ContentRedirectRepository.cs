using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Factories;
using Nikcio.UHeadless.Content.Models;

namespace Nikcio.UHeadless.Content.Repositories;

/// <inheritdoc/>
public class ContentRedirectRepository<TContentRedirect> : IContentRedirectRepository<TContentRedirect>
    where TContentRedirect : IContentRedirect
{
    /// <summary>
    /// A factory for creating content redirects
    /// </summary>
    protected IContentRedirectFactory<TContentRedirect> contentRedirectFactory { get; }

    /// <inheritdoc/>
    public ContentRedirectRepository(IContentRedirectFactory<TContentRedirect> contentRedirectFactory)
    {
        this.contentRedirectFactory = contentRedirectFactory;
    }

    /// <inheritdoc/>
    public TContentRedirect? GetContentRedirect(CreateContentRedirect createContentRedirect)
    {
        return contentRedirectFactory.CreateContentRedirect(createContentRedirect);
    }
}
