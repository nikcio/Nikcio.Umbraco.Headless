using Nikcio.UHeadless.Core.Commands;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Properties.Commands;

/// <summary>
/// Command for creating a property value
/// </summary>
public class CreatePropertyValue : ICommand
{
    /// <inheritdoc/>
    public CreatePropertyValue(IPublishedContent content, IPublishedProperty property, string? culture, string? segment, IPublishedValueFallback publishedValueFallback, Fallback? fallback)
    {
        Content = content;
        Property = property;
        Culture = culture;
        Segment = segment;
        PublishedValueFallback = publishedValueFallback;
        Fallback = fallback ?? default;
    }

    /// <summary>
    /// The <see cref="IPublishedContent"/>
    /// </summary>
    public virtual IPublishedContent Content { get; set; }

    /// <summary>
    /// The <see cref="IPublishedProperty"/>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This is the standard name for a property in Umbraco and would be confusing to change.")]
    public virtual IPublishedProperty Property { get; set; }

    /// <summary>
    /// The culture
    /// </summary>
    public virtual string? Culture { get; set; }

    /// <summary>
    /// The segment
    /// </summary>
    public virtual string? Segment { get; set; }

    /// <summary>
    /// The published value fallback
    /// </summary>
    public virtual IPublishedValueFallback PublishedValueFallback { get; set; }

    /// <summary>
    /// The fallback tactic
    /// </summary>
    public virtual Fallback Fallback { get; set; }
}
