using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
public class ContentPickerResponse : PropertyValue
{
    private readonly IEnumerable<IPublishedContent> _publishedContentItems;
    private readonly bool _isMultiple;

    /// <summary>
    /// Whether the picker has multiple items
    /// </summary>
    [GraphQLDescription("Whether the picker has multiple items.")]
    public bool IsMultiple => _isMultiple;

    /// <summary>
    /// Gets the content items of a picker
    /// </summary>
    [GraphQLDescription("Gets the content items of a picker.")]
    public List<ContentPickerItemResponse>? Items => _publishedContentItems.Select(publishedContent =>
    {
        return new ContentPickerItemResponse(publishedContent, Culture, ResolverContext);
    }).ToList();

    public ContentPickerResponse(CreateCommand command) : base(command)
    {
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            _publishedContentItems = new List<IPublishedContent> { publishedContent };
            _isMultiple = false;
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            _publishedContentItems = publishedContentItems;
            _isMultiple = true;
        }
        else
        {
            _publishedContentItems = new List<IPublishedContent>();
            _isMultiple = false;
        }
    }
}

/// <summary>
/// Represents a content picker item
/// </summary>
[GraphQLDescription("Represents a content picker item.")]
public class ContentPickerItemResponse
{
    private readonly IPublishedContent _publishedContent;
    private readonly string? _culture;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IResolverContext _resolverContext;

    /// <summary>
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public string? UrlSegment => _publishedContent?.UrlSegment(_variationContextAccessor, _culture);

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    public string Url(UrlMode urlMode)
    {
        return _publishedContent.Url(_culture, urlMode);
    }

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public string? Name => _publishedContent?.Name(_variationContextAccessor, _culture);

    /// <summary>
    /// Gets the id of a content item
    /// </summary>
    [GraphQLDescription("Gets the id of a content item.")]
    public int Id => _publishedContent.Id;

    /// <summary>
    /// Gets the key of a content item
    /// </summary>
    [GraphQLDescription("Gets the key of a content item.")]
    public Guid Key => _publishedContent.Key;

    /// <summary>
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.PublishedContent, _publishedContent);
        return new TypedProperties();
    }

    public ContentPickerItemResponse(IPublishedContent publishedContent, string? culture, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        _publishedContent = publishedContent;
        _culture = culture;
        _variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
        _resolverContext = resolverContext;
    }
}
