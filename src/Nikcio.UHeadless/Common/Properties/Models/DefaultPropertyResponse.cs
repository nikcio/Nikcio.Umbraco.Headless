using System.Collections;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// A catch all property value that simply returns the value of the property. This is all that is needed for simple properties that doesn't need any special handling or formatting.
/// </summary>
[GraphQLDescription("A catch all property value that simply returns the value of the property. This is all that is needed for simple properties that doesn't need any special handling or formatting.")]
public class DefaultPropertyResponse : PropertyValue
{
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    [GraphQLType(typeof(AnyType))]
    [GraphQLDescription("Gets the value of the property.")]
    public object? Value()
    {
        object? value = PublishedProperty.Value(PublishedValueFallback, Culture, Segment, Fallback);

        if (value is not string && value is IEnumerable list && !list.GetEnumerator().MoveNext())
        {
            value = new List<object>();
        }

        return value;
    }

    /// <inheritdoc/>
    public DefaultPropertyResponse(CreateCommand command) : base(command)
    {
    }
}
