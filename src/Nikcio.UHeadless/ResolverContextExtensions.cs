using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless;

public static class ResolverContextExtensions
{
    public static bool IncludePreview(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState(ContextDataKeys.IncludePreview, _ => false);
    }

    public static string? Culture(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Culture, _ => null);
    }

    public static string? Segment(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Segment, _ => null);
    }

    public static Fallback Fallback(this IResolverContext resolverContext)
    {
        return resolverContext.GetOrSetScopedState<Fallback>(ContextDataKeys.Fallback, _ => default);
    }

    public static IPublishedContent PublishedContent(this IResolverContext resolverContext)
    {
        return resolverContext.GetScopedState<IPublishedContent?>(ContextDataKeys.PublishedContent)
            ?? throw new InvalidOperationException("The published content is not available in scoped data.");
    }
}
