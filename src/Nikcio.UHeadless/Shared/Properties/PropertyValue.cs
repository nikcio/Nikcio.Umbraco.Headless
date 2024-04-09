using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Shared.Properties;

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
        ResolverContext = command.ResolverContext;

        PublishedContent = ResolverContext.GetScopedState<IPublishedContent?>(ContextDataKeys.PublishedContent)
            ?? throw new InvalidOperationException("The published content is not available in scoped data.");
        Culture = ResolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        Segment = ResolverContext.GetScopedState<string?>(ContextDataKeys.Segment);
        Fallback = ResolverContext.GetScopedState<Fallback?>(ContextDataKeys.Fallback) ?? default;
        IsPreview = ResolverContext.GetScopedState<bool>(ContextDataKeys.IsPreview);
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
    protected string? Culture { get; }

    /// <summary>
    /// The segment
    /// </summary>
    protected string? Segment { get; }

    /// <summary>
    /// The fallback tactic
    /// </summary>
    protected Fallback Fallback { get; }

    /// <summary>
    /// Determines if the query allows fetching preview content
    /// </summary>
    protected bool IsPreview { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    protected IResolverContext ResolverContext { get; }

    /// <summary>
    /// The model of the property value
    /// </summary>
    public string Model => GetType().Name;
}
