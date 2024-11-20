using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;

/// <summary>
/// Represents a media item
/// </summary>
[GraphQLDescription("Represents a media item.")]
[Obsolete("Use Defaults.Properties.MediaPickerItem instead.")]
public class BasicMediaPickerItem : MediaPickerItem
{
    public BasicMediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext) : base(publishedContent, resolverContext)
    {
    }
}
