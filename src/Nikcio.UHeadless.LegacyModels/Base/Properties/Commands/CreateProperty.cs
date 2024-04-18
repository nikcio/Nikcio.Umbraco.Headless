using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Properties.Commands;

/// <summary>
/// A command for creating a property
/// </summary>
[Obsolete("Convert to using typed properties instead")]
public class CreateProperty
{
    /// <inheritdoc/>
    public CreateProperty(IPublishedProperty publishedProperty, string? culture, IPublishedContent publishedContent, string? segment, IPublishedValueFallback publishedValueFallback, Fallback? fallback)
    {
        PublishedProperty = publishedProperty;
        Culture = culture;
        PublishedContent = publishedContent;
        Segment = segment;
        PublishedValueFallback = publishedValueFallback;
        Fallback = fallback;
    }

    /// <summary>
    /// The culture
    /// </summary>
    public string? Culture { get; }

    /// <summary>
    /// The published content
    /// </summary>
    public IPublishedContent PublishedContent { get; }

    /// <summary>
    /// The published property
    /// </summary>
    public IPublishedProperty PublishedProperty { get; }

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
    public virtual Fallback? Fallback { get; set; }
}
