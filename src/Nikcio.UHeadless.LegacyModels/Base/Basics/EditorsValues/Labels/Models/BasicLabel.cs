using HotChocolate;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.Labels.Models;

/// <summary>
/// Represents a label property value
/// </summary>
[GraphQLDescription("Represents a date time property value.")]
[Obsolete("Use Defaults.Properties.Label instead.")]
public class BasicLabel : Label
{
    public BasicLabel(CreateCommand command) : base(command)
    {
    }
}
