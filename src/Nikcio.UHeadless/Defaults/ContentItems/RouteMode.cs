namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Modes for requesting by route
/// </summary>
[GraphQLDescription("Modes for requesting by route")]
public enum RouteMode
{
    /// <summary>
    /// Cache only will only look in the content cache for the url
    /// </summary>
    [GraphQLDescription("Cache only will only look in the content cache for the url")]
    CacheOnly,

    /// <summary>
    /// Routing will use routing to determine a route. This will also show redirects
    /// </summary>
    [GraphQLDescription("Routing will use routing to determine a route. This will also show redirects")]
    Routing,

    /// <summary>
    /// Routing or cache will first use routing to find content and then use the cache if none is found. This also shows redirects
    /// </summary>
    [GraphQLDescription("Routing or cache will first use routing to find content and then use the cache if none is found. This also shows redirects")]
    RoutingOrCache,

    /// <summary>
    /// Cache or routing will first use the content cache to find content and then use routing. This will only find redirects if no content is found in the content cache
    /// </summary>
    [GraphQLDescription("Cache or routing will first use the content cache to find content and then use routing. This will only find redirects if no content is found in the content cache")]
    CacheOrRouting,
}
