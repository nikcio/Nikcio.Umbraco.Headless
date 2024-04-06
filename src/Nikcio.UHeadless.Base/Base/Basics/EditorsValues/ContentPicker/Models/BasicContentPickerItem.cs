using Nikcio.UHeadless.Base.Properties.EditorsValues.ContentPicker.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.ContentPicker.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models;

/// <summary>
/// Represents a content picker item
/// </summary>
[GraphQLDescription("Represents a content picker item.")]
public class BasicContentPickerItem : ContentPickerItem
{
    /// <summary>
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public virtual string? UrlSegment => Content?.UrlSegment(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    public virtual string Url => Content.Url(Culture, UrlMode.Default);

    /// <summary>
    /// Gets the absolute url of a content item
    /// </summary>
    [GraphQLDescription("Gets the absolute url of a content item.")]
    public virtual string AbsoluteUrl => Content.Url(Culture, UrlMode.Absolute);

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public virtual string? Name => Content?.Name(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the id of a content item
    /// </summary>
    [GraphQLDescription("Gets the id of a content item.")]
    public virtual int Id => Content.Id;

    /// <summary>
    /// Gets the key of a content item
    /// </summary>
    [GraphQLDescription("Gets the key of a content item.")]
    public virtual Guid Key => Content.Key;

    /// <summary>
    /// The <see cref="IPublishedContent"/>
    /// </summary>
    [GraphQLIgnore]
    protected virtual IPublishedContent Content { get; set; }

    /// <summary>
    /// The variation context accessor
    /// </summary>
    [GraphQLIgnore]
    protected virtual IVariationContextAccessor VariationContextAccessor { get; set; }

    /// <summary>
    /// The culture
    /// </summary>
    [GraphQLIgnore]
    protected virtual string? Culture { get; set; }

    /// <inheritdoc/>
    public BasicContentPickerItem(CreateContentPickerItem createContentPickerItem) : base(createContentPickerItem)
    {
        ArgumentNullException.ThrowIfNull(createContentPickerItem);

        Content = createContentPickerItem.PublishedContent;
        VariationContextAccessor = createContentPickerItem.VariationContextAccessor;
    }
}
