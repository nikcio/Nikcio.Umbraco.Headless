using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.DateTimePicker.Models;

/// <summary>
/// Represents a date time property value
/// </summary>
[GraphQLDescription("Represents a date time property value.")]
public class BasicDateTimePicker : PropertyValue
{
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    [GraphQLDescription("Gets the value of the property.")]
    public virtual DateTime? Value { get; set; }

    /// <inheritdoc/>
    public BasicDateTimePicker(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        DateTime? value = createPropertyValue.Property.Value<DateTime?>(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);

        if (value == default(DateTime))
        {
            Value = null;
        }
        else
        {
            Value = value;
        }
    }
}
