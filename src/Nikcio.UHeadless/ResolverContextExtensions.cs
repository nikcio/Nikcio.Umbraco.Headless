using HotChocolate.Resolvers;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless;

public static class ResolverContextExtensions
{
    internal static QueryContext Initialize(this IResolverContext resolverContext, QueryContext? queryContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentNullException.ThrowIfNull(queryContext);

        resolverContext.SetScopedState(ContextDataKeys.IncludePreview, queryContext.IncludePreview ?? false);
        resolverContext.SetScopedState(ContextDataKeys.Culture, queryContext.Culture);
        resolverContext.SetScopedState(ContextDataKeys.Segment, queryContext.Segment);
        resolverContext.SetScopedState(ContextDataKeys.Fallback, queryContext.Fallbacks?.ToFallback() ?? default);
        resolverContext.SetScopedState(ContextDataKeys.BaseUrl, queryContext.BaseUrl);

        return queryContext ?? new();
    }

    /// <summary>
    /// Gets whether to include preview content in the query
    /// </summary>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    public static bool IncludePreview(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState(ContextDataKeys.IncludePreview, _ => false);
    }

    /// <summary>
    /// Gets the culture of the query
    /// </summary>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    public static string? Culture(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Culture, _ => null);
    }

    /// <summary>
    /// Gets the segment of the query
    /// </summary>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    public static string? Segment(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Segment, _ => null);
    }

    /// <summary>
    /// Gets the fallback of the query to be used for property values
    /// </summary>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    public static Fallback Fallback(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState<Fallback?>(ContextDataKeys.Fallback, _ => default) ?? default;
    }

    /// <summary>
    /// Gets the base URL of the request. Used to get the correct URLs on content items.
    /// </summary>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    public static string BaseUrl(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState(ContextDataKeys.BaseUrl, _ => "");
    }

    /// <summary>
    /// Gets the published content from the scoped data
    /// </summary>
    /// <remarks>
    /// The published content item can be different based on where in the query we are. This is what allows for block lists and block grids to work.
    /// </remarks>
    /// <exception cref="InvalidOperationException">In case the published content isn't available.</exception>
    public static IPublishedContent PublishedContent(this IResolverContext resolverContext)
    {
        return resolverContext.GetScopedState<IPublishedContent?>(ContextDataKeys.PublishedContent)
            ?? throw new InvalidOperationException("The published content is not available in scoped data.");
    }
}
