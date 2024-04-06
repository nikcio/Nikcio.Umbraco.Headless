using System.Collections;
using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.Fallback.Models;

/// <summary>
/// Represents a basic property value
/// </summary>
[GraphQLDescription("Represents a basic property value.")]
public class BasicPropertyValue : PropertyValue
{
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    [GraphQLType(typeof(AnyType))]
    [GraphQLDescription("Gets the value of the property.")]
    public virtual object? Value { get; set; }

    /// <inheritdoc/>
    public BasicPropertyValue(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        Value = createPropertyValue.Property.Value(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);

        if (Value is IEnumerable list && !list.GetEnumerator().MoveNext())
        {
            Value = new List<object>();
        }
    }
}
