using HotChocolate;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.DateTimePicker.Models;

/// <summary>
/// Represents a date time property value
/// </summary>
[GraphQLDescription("Represents a date time property value.")]
[Obsolete("Use Defaults.Properties.DateTimePicker instead.")]
public class BasicDateTimePicker : Defaults.Properties.DateTimePicker
{
    public BasicDateTimePicker(CreateCommand command) : base(command)
    {
    }
}
