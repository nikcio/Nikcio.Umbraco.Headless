using System.Collections;
using HotChocolate;
using HotChocolate.Types;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// A catch all property value that simply returns the value of the property. This is all that is needed for simple properties that doesn't need any special handling or formatting.
/// </summary>
[GraphQLDescription("A catch all property value that simply returns the value of the property. This is all that is needed for simple properties that doesn't need any special handling or formatting.")]
public class DefaultProperty : PropertyValue
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
    public DefaultProperty(CreateCommand command) : base(command)
    {
    }
}
