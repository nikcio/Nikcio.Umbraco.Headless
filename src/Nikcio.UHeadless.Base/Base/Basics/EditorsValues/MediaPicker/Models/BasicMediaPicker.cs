using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.MediaPicker.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.MediaPicker.Models;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
public class BasicMediaPicker : BasicMediaPicker<BasicMediaPickerItem>
{
    /// <inheritdoc/>
    public BasicMediaPicker(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
    }
}

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
public class BasicMediaPicker<TMediaItem> : PropertyValue
    where TMediaItem : MediaPickerItem
{
    /// <summary>
    /// Gets the media items of a picker
    /// </summary>
    [GraphQLDescription("Gets the media items of a picker.")]
    public virtual List<TMediaItem> MediaItems { get; set; } = new();

    /// <inheritdoc/>
    public BasicMediaPicker(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue)
    {
        object? value = createPropertyValue.Property.Value(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);
        if (value is IPublishedContent mediaItem)
        {
            AddMediaPickerItem(dependencyReflectorFactory, mediaItem, createPropertyValue.Culture);
        }
        else if (value is IEnumerable<IPublishedContent> mediaItems && mediaItems.Any())
        {
            foreach (IPublishedContent media in mediaItems)
            {
                AddMediaPickerItem(dependencyReflectorFactory, media, createPropertyValue.Culture);
            }
        }
    }

    /// <summary>
    /// Adds a media picker item to media items
    /// </summary>
    /// <param name="dependencyReflectorFactory"></param>
    /// <param name="media"></param>
    /// <param name="culture"></param>
    protected void AddMediaPickerItem(IDependencyReflectorFactory dependencyReflectorFactory, IPublishedContent media, string? culture)
    {
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TMediaItem? mediaPickerItem = dependencyReflectorFactory.GetReflectedType<TMediaItem>(typeof(TMediaItem), new object[] { new CreateMediaPickerItem(media, culture) });
        if (mediaPickerItem != null)
        {
            MediaItems.Add(mediaPickerItem);
        }
    }
}
