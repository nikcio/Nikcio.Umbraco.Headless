using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents a date time property value
/// </summary>
[GraphQLDescription("Represents a date time property value.")]
public class DateTimePickerResponse : PropertyValue
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

    public DateTimePickerResponse(CreateCommand command) : base(command)
    {
    }
}
