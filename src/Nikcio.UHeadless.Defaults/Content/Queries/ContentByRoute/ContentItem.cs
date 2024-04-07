using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Shared.Properties;

namespace Nikcio.UHeadless.Defaults.Content.Queries.ContentByRoute;

public partial class ContentItem : ContentItemBase
{
    public ContentItem(CreateCommand command) : base(command)
    {
    }

    public string? Title => PublishedContent?.Name;

    public TypedProperties Properties => new();
}
