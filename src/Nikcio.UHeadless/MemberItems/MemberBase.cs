using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Members;

public abstract partial class MemberItemBase
{
    protected MemberItemBase(CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        PublishedContent = command.PublishedContent;
        ResolverContext = command.ResolverContext;

        Culture = ResolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Culture, _ => null);
        Segment = ResolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Segment, _ => null);
        Fallback = ResolverContext.GetOrSetScopedState<Fallback?>(ContextDataKeys.Fallback, _ => null);
        IncludePreview = ResolverContext.GetOrSetScopedState(ContextDataKeys.IncludePreview, _ => false);

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

    /// <summary>
    /// Whether to include preview content
    /// </summary>
    protected bool IncludePreview { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    protected IResolverContext ResolverContext { get; }
}
