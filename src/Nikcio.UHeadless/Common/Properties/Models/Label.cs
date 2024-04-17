using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

public class Label : PropertyValue
{
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    [GraphQLType(typeof(AnyType))]
    [GraphQLDescription("Gets the value of the property.")]
    public object? Value()
    {
        object? value = PublishedProperty.Value(PublishedValueFallback, Culture, Segment, Fallback);

        if (value is DateTime dateTimeValue)
        {
            return dateTimeValue == default ? null : dateTimeValue;
        }
        else
        {
            return value;
        }
    }

    public Label(CreateCommand command) : base(command)
    {
    }
}
