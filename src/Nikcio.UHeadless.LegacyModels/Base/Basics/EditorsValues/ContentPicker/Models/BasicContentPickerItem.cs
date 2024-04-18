using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models;

/// <summary>
/// Represents a content picker item
/// </summary>
[GraphQLDescription("Represents a content picker item.")]
[Obsolete("Use Defaults.Properties.ContentPickerItem instead. The main differences here is the Url property that now takes an argument of UrlMode meaning we don't need multiple properties for the different modes we need.")]
public class BasicContentPickerItem : ContentPickerItem
{
    public BasicContentPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext) : base(publishedContent, resolverContext)
    {
    }

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    [Obsolete("Use the underlying Url() method instead this requires an argument with the UrlMode.")]
    public new string Url => PublishedContent.Url(Culture, UrlMode.Default);

    /// <summary>
    /// Gets the absolute url of a content item
    /// </summary>
    [GraphQLDescription("Gets the absolute url of a content item.")]
    [Obsolete("Use the underlying Url() method instead this requires an argument with the UrlMode.")]
    public virtual string AbsoluteUrl => PublishedContent.Url(Culture, UrlMode.Absolute);
}
