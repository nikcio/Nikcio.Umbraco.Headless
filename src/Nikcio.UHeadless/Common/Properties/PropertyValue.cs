using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common.Directives;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Common.Properties;

/// <summary>
/// A base for property values
/// </summary>
[InterfaceType(nameof(PropertyValue))]
public abstract partial class PropertyValue
{
    /// <inheritdoc/>
    protected PropertyValue(CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        PublishedProperty = command.PublishedProperty;
        PublishedValueFallback = command.PublishedValueFallback;

        PublishedContent = command.ResolverContext.GetScopedState<IPublishedContent?>(ContextDataKeys.PublishedContent)
            ?? throw new InvalidOperationException("The published content is not available in scoped data.");
    }

    /// <summary>
    /// The <see cref="IPublishedContent"/>
    /// </summary>
    protected IPublishedContent PublishedContent { get; }

    /// <summary>
    /// The <see cref="IPublishedProperty"/>
    /// </summary>
    protected IPublishedProperty PublishedProperty { get; }

    /// <summary>
    /// The published value fallback
    /// </summary>
    protected IPublishedValueFallback PublishedValueFallback { get; }

    /// <summary>
    /// The culture
    /// </summary>
    protected static string? Culture(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        UseDirectivesAttribute.Middleware.InvokeDirectives(resolverContext);
        return resolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Culture, _ => null);
    }

    /// <summary>
    /// The segment
    /// </summary>
    protected static string? Segment(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        UseDirectivesAttribute.Middleware.InvokeDirectives(resolverContext);
        return resolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Segment, _ => null);
    }

    /// <summary>
    /// The fallback tactic
    /// </summary>
    protected static Fallback Fallback(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        UseDirectivesAttribute.Middleware.InvokeDirectives(resolverContext);
        return resolverContext.GetOrSetScopedState<Fallback>(ContextDataKeys.Fallback, _ => default);
    }

    /// <summary>
    /// Whether to include preview content
    /// </summary>
    protected static bool IncludePreview(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        UseDirectivesAttribute.Middleware.InvokeDirectives(resolverContext);
        return resolverContext.GetOrSetScopedState(ContextDataKeys.IncludePreview, _ => false);
    }

    /// <summary>
    /// The model of the property value
    /// </summary>
    public string Model => GetType().Name;
}
