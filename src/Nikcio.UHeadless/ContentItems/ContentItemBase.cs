using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Directives;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentItems;

public abstract partial class ContentItemBase
{
    protected ContentItemBase(CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        PublishedContent = command.PublishedContent;
        ResolverContext = command.ResolverContext;

        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
    }

    /// <summary>
    /// The published content
    /// </summary>
    protected IPublishedContent? PublishedContent { get; }

    /// <summary>
    /// The culture
    /// </summary>
    protected string? Culture => ResolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Culture, _ => null);

    /// <summary>
    /// The segment
    /// </summary>
    protected string? Segment => ResolverContext.GetOrSetScopedState<string?>(ContextDataKeys.Segment, _ => null);

    /// <summary>
    /// The fallback tactic
    /// </summary>
    protected Fallback? Fallback => ResolverContext.GetOrSetScopedState<Fallback?>(ContextDataKeys.Fallback, _ => null);

    /// <summary>
    /// Whether to include preview content
    /// </summary>
    protected bool IncludePreview => ResolverContext.GetOrSetScopedState(ContextDataKeys.IncludePreview, _ => false);

    /// <summary>
    /// The resolver context
    /// </summary>
    protected IResolverContext ResolverContext { get; }
}
