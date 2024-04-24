using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
public class NestedContent : NestedContent<NestedContentItem>
{
    public NestedContent(CreateCommand command) : base(command)
    {
    }

    protected override NestedContentItem CreateNestedContentItem(IPublishedElement publishedElement, IResolverContext resolverContext)
    {
        return new NestedContentItem(publishedElement, resolverContext);
    }
}

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
public abstract class NestedContent<TNestedContentItem> : PropertyValue
    where TNestedContentItem : class
{
    /// <summary>
    /// The published elements
    /// </summary>
    /// <value></value>
    protected List<IPublishedElement> PublishedElements { get; }

    /// <summary>
    /// Gets the elements of a nested content
    /// </summary>
    [GraphQLDescription("Gets the elements of a nested content.")]
    public List<NestedContentItem> Elements(IResolverContext resolverContext)
    {
        return PublishedElements.Select(publishedElement =>
        {
            return new NestedContentItem(publishedElement, resolverContext);
        }).ToList();
    }

    protected NestedContent(CreateCommand command) : base(command)
    {
        IResolverContext resolverContext = command.ResolverContext;
        object? publishedElementsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture(resolverContext), Segment(resolverContext), Fallback(resolverContext));

        if (publishedElementsAsObject is IPublishedElement publishedElement)
        {
            PublishedElements = new List<IPublishedElement> { publishedElement };
        }
        else if (publishedElementsAsObject is IEnumerable<IPublishedElement> publishedElements)
        {
            PublishedElements = publishedElements.ToList();
        }
        else
        {
            PublishedElements = new List<IPublishedElement>();
        }
    }

    /// <summary>
    /// Creates a nested content item
    /// </summary>
    /// <param name="publishedElement"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TNestedContentItem CreateNestedContentItem(IPublishedElement publishedElement, IResolverContext resolverContext);
}

public class NestedContentItem
{
    /// <summary>
    /// The published element
    /// </summary>
    /// <value></value>
    protected IPublishedElement PublishedElement { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    /// <value></value>
    protected IResolverContext ResolverContext { get; }

    /// <summary>
    /// Gets the properties of the nested content
    /// </summary>
    [GraphQLDescription("Gets the properties of the nested content.")]
    public TypedNestedContentProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.NestedContent, PublishedElement);
        ResolverContext.SetScopedState(ContextDataKeys.NestedContentPropertyName, nameof(Properties));
        return new TypedNestedContentProperties();
    }

    public NestedContentItem(IPublishedElement publishedElement, IResolverContext resolverContext)
    {
        PublishedElement = publishedElement;
        ResolverContext = resolverContext;
    }
}
