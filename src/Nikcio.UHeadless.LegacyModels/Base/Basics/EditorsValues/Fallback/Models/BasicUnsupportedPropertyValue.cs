using HotChocolate;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.Fallback.Models;

/// <summary>
/// Represents an unsupported property value
/// </summary>
[GraphQLDescription("Represents an unsupported property value.")]
[Obsolete("Use Defaults.Properties.UnsupportedProperty instead.")]
public class BasicUnsupportedPropertyValue : UnsupportedProperty
{
    public BasicUnsupportedPropertyValue(CreateCommand command) : base(command)
    {
    }
}
