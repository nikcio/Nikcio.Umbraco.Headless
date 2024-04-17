using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
public class NestedContentResponse : PropertyValue
{
    private readonly IEnumerable<IPublishedElement> _publishedElements;

    /// <summary>
    /// Gets the elements of a nested content
    /// </summary>
    [GraphQLDescription("Gets the elements of a nested content.")]
    public List<NestedContentItemResponse> Elements()
    {
        return _publishedElements.Select(publishedElement =>
        {
            return new NestedContentItemResponse(publishedElement, ResolverContext);
        }).ToList();
    }

    public NestedContentResponse(CreateCommand command) : base(command)
    {
        object? publishedElementsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedElementsAsObject is IPublishedElement publishedElement)
        {
            _publishedElements = new List<IPublishedElement> { publishedElement };
        }
        else if (publishedElementsAsObject is IEnumerable<IPublishedElement> publishedElements)
        {
            _publishedElements = publishedElements;
        }
        else
        {
            _publishedElements = new List<IPublishedElement>();
        }
    }
}

public class NestedContentItemResponse
{
    private readonly IPublishedElement _publishedElement;
    private readonly IResolverContext _resolverContext;

    /// <summary>
    /// Gets the properties of the nested content
    /// </summary>
    [GraphQLDescription("Gets the properties of the nested content.")]
    public virtual TypedNestedContentProperties Properties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.NestedContent, _publishedElement);
        _resolverContext.SetScopedState(ContextDataKeys.NestedContentPropertyName, nameof(Properties));
        return new TypedNestedContentProperties();
    }

    public NestedContentItemResponse(IPublishedElement publishedElement, IResolverContext resolverContext)
    {
        _publishedElement = publishedElement;
        _resolverContext = resolverContext;
    }
}
