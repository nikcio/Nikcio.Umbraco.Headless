using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.Labels.Models;

/// <summary>
/// Represents a label property value
/// </summary>
[GraphQLDescription("Represents a date time property value.")]
public class BasicLabel : PropertyValue
{
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    [GraphQLType(typeof(AnyType))]
    [GraphQLDescription("Gets the value of the property.")]
    public virtual object? Value { get; set; }

    /// <inheritdoc/>
    public BasicLabel(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        object? value = createPropertyValue.Property.Value(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);
        if (value != null)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue == default)
                {
                    Value = null;
                }
                else
                {
                    Value = dateTimeValue;
                }
            }
            else
            {
                Value = value;
            }
        }
    }
}
