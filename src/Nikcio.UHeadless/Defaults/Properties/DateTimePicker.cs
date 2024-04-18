using Nikcio.UHeadless.Common.Properties;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a date time property value
/// </summary>
[GraphQLDescription("Represents a date time property value.")]
public class DateTimePicker : PropertyValue
{
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    [GraphQLDescription("Gets the value of the property.")]
    public DateTime? Value()
    {
        DateTime? dateTimeValue = PublishedProperty.Value<DateTime?>(PublishedValueFallback, Culture, Segment, Fallback);
        return dateTimeValue == default(DateTime) ? null : dateTimeValue;
    }

    public DateTimePicker(CreateCommand command) : base(command)
    {
    }
}
