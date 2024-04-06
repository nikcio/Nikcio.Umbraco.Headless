using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.ContentPicker.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.ContentPicker.Models;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models;

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
public class BasicContentPicker : BasicContentPicker<BasicContentPickerItem>
{
    /// <inheritdoc/>
    public BasicContentPicker(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory, IVariationContextAccessor variationContextAccessor) : base(createPropertyValue, dependencyReflectorFactory, variationContextAccessor)
    {
    }
}

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
public class BasicContentPicker<TContentPickerItem> : PropertyValue
    where TContentPickerItem : ContentPickerItem
{
    /// <summary>
    /// Gets the list of content
    /// </summary>
    [GraphQLDescription("Gets the list of content.")]
    public virtual List<TContentPickerItem> ContentList { get; set; } = new();

    /// <inheritdoc/>
    public BasicContentPicker(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory, IVariationContextAccessor variationContextAccessor) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        object? objectValue = createPropertyValue.Property.Value(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);
        if (objectValue is IPublishedContent content)
        {
            AddContentPickerItem(dependencyReflectorFactory, content, variationContextAccessor, createPropertyValue.Culture);
        } else if (objectValue is IEnumerable<IPublishedContent> contentItems)
        {
            foreach (IPublishedContent contentItem in contentItems)
            {
                AddContentPickerItem(dependencyReflectorFactory, contentItem, variationContextAccessor, createPropertyValue.Culture);
            }
        }
    }

    /// <summary>
    /// Adds a content picker item to the content list
    /// </summary>
    /// <param name="dependencyReflectorFactory"></param>
    /// <param name="content"></param>
    /// <param name="variationContextAccessor"></param>
    /// <param name="culture"></param>
    protected void AddContentPickerItem(IDependencyReflectorFactory dependencyReflectorFactory, IPublishedContent content, IVariationContextAccessor variationContextAccessor, string? culture)
    {
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TContentPickerItem? contentPickerItem = dependencyReflectorFactory.GetReflectedType<TContentPickerItem>(typeof(TContentPickerItem), new object[] { new CreateContentPickerItem(content, variationContextAccessor, culture) });
        if (contentPickerItem != null)
        {
            ContentList.Add(contentPickerItem);
        }
    }
}
