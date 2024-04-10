using HotChocolate.Resolvers;
using Nikcio.UHeadless.Shared;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentItems;

public abstract partial class ContentItemBase
{
    protected ContentItemBase(CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        PublishedContent = command.PublishedContent;
        ResolverContext = command.ResolverContext;

        Culture = ResolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Culture, _ => null);
        Segment = ResolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Segment, _ => null);
        Fallback = ResolverContext.GetOrSetScopedState<Fallback?>(ContextDataKeys.Fallback, _ => null);
        IsPreview = ResolverContext.GetOrSetScopedState(ContextDataKeys.IsPreview, _ => false);

        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
    }

    /// <summary>
    /// The published content
    /// </summary>
    protected IPublishedContent? PublishedContent { get; }

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
    protected Fallback? Fallback { get; }

    protected bool IsPreview { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    protected IResolverContext ResolverContext { get; }
}
