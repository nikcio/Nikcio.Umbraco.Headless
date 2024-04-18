using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentTypes.Models;

/// <inheritdoc/>
public abstract class ContentType : IContentType
{
    /// <inheritdoc/>
    protected ContentType(IPublishedContentType contentType)
    {
        PublishedContentType = contentType;
    }

    /// <summary>
    /// THe publised content type
    /// </summary>
    protected IPublishedContentType PublishedContentType { get; }
}
