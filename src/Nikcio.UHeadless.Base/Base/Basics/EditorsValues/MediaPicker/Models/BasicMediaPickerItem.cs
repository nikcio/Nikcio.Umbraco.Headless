using Nikcio.UHeadless.Base.Properties.EditorsValues.MediaPicker.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.MediaPicker.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;

/// <summary>
/// Represents a media item
/// </summary>
[GraphQLDescription("Represents a media item.")]
public class BasicMediaPickerItem : MediaPickerItem
{
    /// <summary>
    /// Gets the absolute url of a media item
    /// </summary>
    [GraphQLDescription("Gets the absolute url of a media item.")]
    public virtual string Url { get; set; }

    /// <summary>
    /// Gets the id of a media item
    /// </summary>
    [GraphQLDescription("Gets the id of a media item.")]
    public virtual int Id { get; set; }

    /// <inheritdoc/>
    public BasicMediaPickerItem(CreateMediaPickerItem createMediaPickerItem) : base(createMediaPickerItem)
    {
        ArgumentNullException.ThrowIfNull(createMediaPickerItem);

        Url = createMediaPickerItem.PublishedContent.MediaUrl(mode: UrlMode.Absolute);
        Id = createMediaPickerItem.PublishedContent.Id;
    }
}
