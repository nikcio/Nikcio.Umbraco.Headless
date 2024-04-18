using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MultiUrlPicker.Models;

/// <summary>
/// Represents a link item
/// </summary>
[GraphQLDescription("Represents a link item.")]
[Obsolete("Use Defaults.Properties.MultiUrlPickerItem instead. The main difference here is that the Url property is slightly changed.")]
public class BasicMultiUrlPickerItem : MultiUrlPickerItem
{
    public BasicMultiUrlPickerItem(IPublishedContent? publishedContent, Link link, IResolverContext resolverContext) : base(publishedContent, link, resolverContext)
    {
    }

    /// <summary>
    /// Gets the url of a link
    /// </summary>
    [GraphQLDescription("Gets the url of a link.")]
    [Obsolete("This changes to require a UrlMode argument that can format the url if the url points to a content item or a media item.")]
    public new string? Url { get; set; }

}
