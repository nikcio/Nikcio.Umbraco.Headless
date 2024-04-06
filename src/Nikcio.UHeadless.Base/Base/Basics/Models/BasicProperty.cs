using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a property.")]
public class BasicProperty : Property
{
    private readonly CreatePropertyValue _createPropertyValue;

    /// <inheritdoc/>
    public BasicProperty(CreateProperty createProperty, IPropertyValueFactory propertyValueFactory) : base(createProperty)
    {
        ArgumentNullException.ThrowIfNull(createProperty, nameof(createProperty));

        publishedProperty = createProperty.PublishedProperty;
        this.propertyValueFactory = propertyValueFactory;
        _createPropertyValue = new CreatePropertyValue(createProperty.PublishedContent, createProperty.PublishedProperty, createProperty.Culture, createProperty.Segment, createProperty.PublishedValueFallback, createProperty.Fallback);
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the alias of a property.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Alias is a name native to the Umbraco landscape and should therefore be easy to understand.")]
    public virtual string? Alias => publishedProperty.Alias;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the value of a property.")]
    public virtual PropertyValue? Value => propertyValueFactory.GetPropertyValue(_createPropertyValue);

    /// <inheritdoc/>
    [GraphQLDescription("Gets the editor alias of a property.")]
    public virtual string? EditorAlias => publishedProperty.PropertyType.EditorAlias;

    /// <summary>
    /// The published property
    /// </summary>
    protected IPublishedProperty publishedProperty { get; }

    /// <summary>
    /// A factory for creating property values
    /// </summary>
    protected IPropertyValueFactory propertyValueFactory { get; }
}
