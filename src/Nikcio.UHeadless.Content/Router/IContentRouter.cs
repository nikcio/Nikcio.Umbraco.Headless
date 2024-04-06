using Nikcio.UHeadless.Content.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace Nikcio.UHeadless.Content.Router;

/// <summary>
/// A router for fetching content
/// </summary>
/// <typeparam name="TContent"></typeparam>
/// <typeparam name="TContentRedirect"></typeparam>
public interface IContentRouter<TContent, TContentRedirect> //TODO: Remove TContentRedirect from interface
    where TContent : IContent
    where TContentRedirect : IContentRedirect
{
    /// <summary>
    /// Gets content by cache
    /// </summary>
    /// <param name="route"></param>
    /// <param name="culture"></param>
    /// <param name="preview"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    TContent? GetContentByRouteCache(string route, string? culture, bool preview, string? segment, Fallback? fallback);

    /// <summary>
    /// Gets content by routing
    /// </summary>
    /// <param name="route"></param>
    /// <param name="baseUrl"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Would be breaking to change")]
    Task<TContent?> GetContentByRouting(string route, string baseUrl, string? culture, string? segment, Fallback? fallback);

    /// <summary>
    /// Gets content descendants by cache
    /// </summary>
    /// <param name="route"></param>
    /// <param name="culture"></param>
    /// <param name="preview"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    IEnumerable<TContent?> GetContentDescendantsByRouteCache(string route, string? culture, bool preview, string? segment, Fallback? fallback);

    /// <summary>
    /// Gets content descendants by routing
    /// </summary>
    /// <param name="route"></param>
    /// <param name="baseUrl"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Would be breaking to change")]
    Task<IEnumerable<TContent?>> GetContentDescendantsByRouting(string route, string baseUrl, string? culture, string? segment, Fallback? fallback);

    /// <summary>
    /// Gets a content redirect result
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    TContent? GetRedirect(IPublishedRequest request);

    /// <summary>
    /// Sets the baseUrl if none is set
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    string SetBaseUrl(string baseUrl);
}
