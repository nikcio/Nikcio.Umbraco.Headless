using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.RichTextEditor.Models;

/// <summary>
/// Represents a rich text editor
/// </summary>
[GraphQLDescription("Represents a rich text editor.")]
[Obsolete("Use Defaults.Properties.RichText instead.")]
public class BasicRichText : RichText
{
    public BasicRichText(CreateCommand command) : base(command)
    {
    }
}
