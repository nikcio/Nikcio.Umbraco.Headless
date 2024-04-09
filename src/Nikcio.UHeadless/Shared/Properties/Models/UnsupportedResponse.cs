namespace Nikcio.UHeadless.Shared.Properties.Models;

/// <summary>
/// Represents an unsupported property value
/// </summary>
[GraphQLDescription("Represents an unsupported property value.")]
public class UnsupportedResponse : PropertyValue
{
    /// <summary>
    /// Gets the message of the property
    /// </summary>
    [GraphQLDescription("Gets the message of the property.")]
    public string Message => $"{PublishedProperty.PropertyType.EditorAlias} is not supported in UHeadless by default. Create your own implementation to use this editor.";

    public UnsupportedResponse(CreateCommand command) : base(command)
    {
    }
}
