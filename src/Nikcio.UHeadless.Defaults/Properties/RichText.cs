using HotChocolate;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a rich text editor
/// </summary>
[GraphQLDescription("Represents a rich text editor.")]
public class RichText : PropertyValue
{
    /// <summary>
    /// Gets the HTML value of the rich text editor or markdown editor
    /// </summary>
    [GraphQLDescription("Gets the HTML value of the rich text editor or markdown editor.")]
    public virtual string? Value => PublishedProperty.Value<IHtmlEncodedString?>(PublishedValueFallback, Culture, Segment, Fallback)?.ToHtmlString();

    /// <summary>
    /// Gets the original value of the rich text editor or markdown editor
    /// </summary>
    [GraphQLDescription("Gets the original value of the rich text editor or markdown editor.")]
    public virtual string? SourceValue => PublishedProperty.GetSourceValue(Culture, Segment)?.ToString();

    public RichText(CreateCommand command) : base(command)
    {
    }
}
