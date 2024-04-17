using HotChocolate;
using Nikcio.UHeadless.Common.Properties;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents an unsupported property value
/// </summary>
[GraphQLDescription("Represents an unsupported property value.")]
public class UnsupportedProperty : PropertyValue
{
    /// <summary>
    /// Gets the message of the property
    /// </summary>
    [GraphQLDescription("Gets the message of the property.")]
    public string Message => $"{PublishedProperty.PropertyType.EditorAlias} is not supported in UHeadless by default. Create your own implementation to use this editor.";

    public UnsupportedProperty(CreateCommand command) : base(command)
    {
    }
}
