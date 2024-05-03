using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentItems;

public abstract partial class ContentItemBase
{
    protected ContentItemBase(CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        PublishedContent = command.PublishedContent;
    }

    /// <summary>
    /// The published content
    /// </summary>
    protected IPublishedContent? PublishedContent { get; }
}
