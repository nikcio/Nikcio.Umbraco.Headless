using Nikcio.UHeadless.ContentTypes.Commands;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentTypes.Models;

/// <inheritdoc/>
public abstract class ContentType : IContentType
{
    /// <inheritdoc/>
    protected ContentType(CreateContentType createContentType)
    {
        ArgumentNullException.ThrowIfNull(createContentType);

        PublishedContentType = createContentType.PublishedContentType;
    }

    /// <summary>
    /// THe publised content type
    /// </summary>
    protected IPublishedContentType PublishedContentType { get; }
}
