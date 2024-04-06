using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Nikcio.UHeadless.Base.Properties.Extensions;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Enums;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.Router;

namespace Nikcio.UHeadless.Content.Queries;

/// <summary>
/// Implements the <see cref="ContentDescendantsByAbsoluteRoute" /> query
/// </summary>
/// <typeparam name="TContent"></typeparam>
/// <typeparam name="TContentRedirect"></typeparam>
public class ContentDescendantsByAbsoluteRouteQuery<TContent, TContentRedirect>
    where TContent : IContent
    where TContentRedirect : IContentRedirect
{
    /// <summary>
    /// Gets content item descendants by an absolute route
    /// </summary>
    /// <param name="contentRouter"></param>
    /// <param name="route"></param>
    /// <param name="baseUrl"></param>
    /// <param name="culture"></param>
    /// <param name="preview"></param>
    /// <param name="routeMode"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets content item descendants by an absolute route.")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Used in GraphQL schema")]
    public virtual async Task<IEnumerable<TContent?>> ContentDescendantsByAbsoluteRoute(
                                                [Service] IContentRouter<TContent, TContentRedirect> contentRouter,
                                                [GraphQLDescription("The route to fetch. Example '/da/frontpage/'.")] string route,
                                                [GraphQLDescription("The base url for the request. Example: 'https://localhost:4000'. Default is the current domain")] string baseUrl = "",
                                                [GraphQLDescription("The culture.")] string? culture = null,
                                                [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
                                                [GraphQLDescription("Modes for requesting by route")] RouteMode routeMode = RouteMode.Routing,
                                                [GraphQLDescription("The property variation segment")] string? segment = null,
                                                [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentRouter);

        if (routeMode is RouteMode.Routing or RouteMode.RoutingOrCache)
        {
            baseUrl = contentRouter.SetBaseUrl(baseUrl);
        }

        return routeMode switch
        {
            RouteMode.Routing => await contentRouter.GetContentDescendantsByRouting(route, baseUrl, culture, segment, fallback?.ToFallback()).ConfigureAwait(false),
            RouteMode.RoutingOrCache => await contentRouter.GetContentDescendantsByRouting(route, baseUrl, culture, segment, fallback?.ToFallback()).ConfigureAwait(false) ?? contentRouter.GetContentDescendantsByRouteCache(route, culture, preview, segment, fallback?.ToFallback()),
            RouteMode.CacheOrRouting => contentRouter.GetContentDescendantsByRouteCache(route, culture, preview, segment, fallback?.ToFallback()) ?? await contentRouter.GetContentDescendantsByRouting(route, baseUrl, culture, segment, fallback?.ToFallback()).ConfigureAwait(false),
            RouteMode.CacheOnly => contentRouter.GetContentDescendantsByRouteCache(route, culture, preview, segment, fallback?.ToFallback()),
            _ => await contentRouter.GetContentDescendantsByRouting(route, baseUrl, culture, segment, fallback?.ToFallback()).ConfigureAwait(false),
        };
    }
}