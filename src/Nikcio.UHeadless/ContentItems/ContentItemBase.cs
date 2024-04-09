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
        Culture = command.Culture;
        Segment = command.Segment;
        Fallback = command.Fallback;
        IsPreview = command.IsPreview;
        ResolverContext = command.ResolverContext;

        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        ResolverContext.SetScopedState(ContextDataKeys.Culture, Culture);
        ResolverContext.SetScopedState(ContextDataKeys.Segment, Segment);
        ResolverContext.SetScopedState(ContextDataKeys.Fallback, Fallback);
        ResolverContext.SetScopedState(ContextDataKeys.IsPreview, IsPreview);
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
