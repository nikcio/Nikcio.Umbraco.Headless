using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Properties;

/// <summary>
/// Represents a property fallback strategy
/// </summary>
public enum PropertyFallback
{
    /// <summary>
    /// Do not fallback
    /// </summary>
    None = 0,

    /// <summary>
    /// Fallback to default value
    /// </summary>
    DefaultValue = 1,

    /// <summary>
    /// Fallback to other languages
    /// </summary>
    Language = 2,

    /// <summary>
    /// Fallback to tree ancestors
    /// </summary>
    Ancestors = 3
}

/// <summary>
/// Property extensions
/// </summary>
public static class PropertyFallbackExtensions
{
    /// <summary>
    /// Transforms <see cref="PropertyFallback" /> to <see cref="Fallback" />
    /// </summary>
    /// <returns></returns>
    public static Fallback ToFallback(this IEnumerable<PropertyFallback> fallbackValues)
    {
        return Fallback.To(fallbackValues.Cast<int>().ToArray());
    }
}
