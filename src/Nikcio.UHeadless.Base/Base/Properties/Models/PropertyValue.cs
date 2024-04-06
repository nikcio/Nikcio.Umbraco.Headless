using Nikcio.UHeadless.Base.Properties.Commands;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Properties.Models;

/// <summary>
/// A base for property values
/// </summary>
[InterfaceType("PropertyValue")]
public abstract class PropertyValue : IDiscoverable
{
    /// <inheritdoc/>
    protected PropertyValue(CreatePropertyValue createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue, nameof(createPropertyValue));

        publishedProperty = createPropertyValue.Property;
    }

    /// <summary>
    /// The published property
    /// </summary>
    protected IPublishedProperty publishedProperty { get; }

    /// <summary>
    /// The model of the property value
    /// </summary>
    public string Model => GetType().Name;
}
