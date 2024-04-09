using Umbraco.Extensions;

namespace Nikcio.UHeadless.Shared.Properties.Models;

public class LabelResponse : PropertyValue
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

    public LabelResponse(CreateCommand command) : base(command)
    {
    }
}
