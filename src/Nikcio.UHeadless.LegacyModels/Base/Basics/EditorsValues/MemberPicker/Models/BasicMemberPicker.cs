namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MemberPicker.Models;

/// <summary>
/// Represents a member picker
/// </summary>
[GraphQLDescription("Represents a member picker.")]
[Obsolete("Use Defaults.Properties.MemberPicker instead")]
public class BasicMemberPicker : BasicMemberPicker<BasicMemberPickerItem>
{
    public BasicMemberPicker(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a member picker
/// </summary>
/// <typeparam name="TMember"></typeparam>
[GraphQLDescription("Represents a member picker.")]
[Obsolete("Use Defaults.Properties.MemberPicker instead")]
public class BasicMemberPicker<TMember> : Defaults.Properties.MemberPicker
    where TMember : class
{
    public BasicMemberPicker(CreateCommand command) : base(command)
    {
    }
}
