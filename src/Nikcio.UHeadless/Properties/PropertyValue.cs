using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Common.Properties;

/// <summary>
/// A base for property values
/// </summary>
[InterfaceType(nameof(PropertyValue))]
public abstract partial class PropertyValue
{
    /// <inheritdoc/>
    protected PropertyValue(CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        PublishedProperty = command.PublishedProperty;
        PublishedValueFallback = command.PublishedValueFallback;

        PublishedContent = command.ResolverContext.PublishedContent();
    }

    /// <summary>
    /// The <see cref="IPublishedContent"/>
    /// </summary>
    protected IPublishedContent PublishedContent { get; }

    /// <summary>
    /// The <see cref="IPublishedProperty"/>
    /// </summary>
    protected IPublishedProperty PublishedProperty { get; }

    /// <summary>
    /// The published value fallback
    /// </summary>
    protected IPublishedValueFallback PublishedValueFallback { get; }

    /// <summary>
    /// The model of the property value
    /// </summary>
    public string Model => GetType().Name;
}
