using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Properties.Factories;

/// <summary>
/// A factory to create properties
/// </summary>
[Obsolete("Convert to using typed properties instead")]
public interface IPropertyFactory<TProperty>
    where TProperty : IProperty
{
    /// <summary>
    /// Gets a property from a <see cref="IPublishedProperty"/>
    /// </summary>
    /// <param name="property">The <see cref="IPublishedProperty"/></param>
    /// <param name="publishedContent">The <see cref="IPublishedContent"/></param>
    /// <param name="culture">The culture</param>
    /// <param name="segment">The segment</param>
    /// <param name="fallback">The fallback tactic</param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This is a Umbraco naming and would be confusing to change")]
    TProperty? GetProperty(IPublishedProperty property, IPublishedContent publishedContent, string? culture, string? segment, Fallback? fallback);

    /// <summary>
    /// Creates properties based on published content
    /// </summary>
    /// <param name="publishedContent"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    IEnumerable<TProperty?> CreateProperties(IPublishedContent publishedContent, string? culture, string? segment, Fallback? fallback);
}
