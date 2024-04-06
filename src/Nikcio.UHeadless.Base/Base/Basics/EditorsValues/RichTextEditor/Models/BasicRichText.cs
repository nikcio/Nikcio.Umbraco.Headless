using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.RichTextEditor.Models;

/// <summary>
/// Represents a rich text editor
/// </summary>
[GraphQLDescription("Represents a rich text editor.")]
public class BasicRichText : PropertyValue
{
    /// <summary>
    /// Gets the HTML value of the rich text editor or markdown editor
    /// </summary>
    [GraphQLDescription("Gets the HTML value of the rich text editor or markdown editor.")]
    public virtual string? Value { get; set; }

    /// <summary>
    /// Gets the original value of the rich text editor or markdown editor
    /// </summary>
    [GraphQLDescription("Gets the original value of the rich text editor or markdown editor.")]
    public virtual string? SourceValue { get; set; }

    /// <inheritdoc/>
    public BasicRichText(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        IHtmlEncodedString? propertyValue = createPropertyValue.Property.Value<IHtmlEncodedString?>(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);
        if (propertyValue == null)
        {
            return;
        }

        Value = propertyValue.ToHtmlString();
        SourceValue = createPropertyValue.Property.GetSourceValue(createPropertyValue.Culture)?.ToString();
    }
}
