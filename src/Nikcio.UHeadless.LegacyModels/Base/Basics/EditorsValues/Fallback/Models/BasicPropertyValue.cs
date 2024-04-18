using HotChocolate;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.Fallback.Models;

/// <summary>
/// Represents a basic property value
/// </summary>
[GraphQLDescription("Represents a basic property value.")]
[Obsolete("Use Defaults.Properties.DefaultProperty instead.")]
public class BasicPropertyValue : DefaultProperty
{
    public BasicPropertyValue(CreateCommand command) : base(command)
    {
    }
}
