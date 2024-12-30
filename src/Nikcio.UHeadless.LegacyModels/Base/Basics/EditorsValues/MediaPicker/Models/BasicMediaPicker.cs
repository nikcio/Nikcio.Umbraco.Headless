using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
[Obsolete("Use Defaults.Properties.MediaPickerItem instead.")]
public class BasicMediaPicker : BasicMediaPicker<BasicMediaPickerItem>
{
    public BasicMediaPicker(CreateCommand command) : base(command)
    {
    }

    protected override BasicMediaPickerItem CreateMediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new BasicMediaPickerItem(publishedContent, resolverContext);
    }
}

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
[Obsolete("Use Defaults.Properties.MediaPicker instead.")]
public abstract class BasicMediaPicker<TMediaItem> : MediaPicker<BasicMediaPickerItem>
    where TMediaItem : class
{
    protected BasicMediaPicker(CreateCommand command) : base(command)
    {
    }
}
