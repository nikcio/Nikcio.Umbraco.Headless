using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.Fallback.Models;

/// <summary>
/// Represents an unsupported property value
/// </summary>
[GraphQLDescription("Represents an unsupported property value.")]
public class BasicUnsupportedPropertyValue : PropertyValue
{
    /// <summary>
    /// Gets the message of the property
    /// </summary>
    [GraphQLDescription("Gets the message of the property.")]
    public virtual string Message { get; set; }

    /// <inheritdoc/>
    public BasicUnsupportedPropertyValue(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        Message = $"{createPropertyValue.Property.PropertyType.EditorAlias} is not supported in UHeadless by default. Create your own implementation to use this editor.";
    }
}
