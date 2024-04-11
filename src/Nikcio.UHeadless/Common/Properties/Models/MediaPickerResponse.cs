using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
public class MediaPickerResponse : PropertyValue
{
    private readonly IEnumerable<IPublishedContent> _publishedMediaItems;
    private readonly bool _isMultiple;
    private readonly IVariationContextAccessor _variationContextAccessor;

    /// <summary>
    /// Gets the media items of a picker
    /// </summary>
    [GraphQLDescription("Gets the media items of a picker.")]
    public List<MediaPickerItemResponse> MediaItems => _publishedMediaItems.Select(publishedMedia =>
    {
        return new MediaPickerItemResponse(publishedMedia, Culture, _variationContextAccessor, ResolverContext);
    }).ToList();

    /// <summary>
    /// Whether the picker has multiple items
    /// </summary>
    [GraphQLDescription("Whether the picker has multiple items.")]
    public bool IsMultiple => _isMultiple;

    public MediaPickerResponse(CreateCommand command, IVariationContextAccessor variationContextAccessor) : base(command)
    {
        _variationContextAccessor = variationContextAccessor;

        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            _publishedMediaItems = new List<IPublishedContent> { publishedContent };
            _isMultiple = false;
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            _publishedMediaItems = publishedContentItems;
            _isMultiple = true;
        }
        else
        {
            _publishedMediaItems = new List<IPublishedContent>();
            _isMultiple = false;
        }
        _variationContextAccessor = variationContextAccessor;
    }
}

/// <summary>
/// Represents a media item
/// </summary>
[GraphQLDescription("Represents a media item.")]
public class MediaPickerItemResponse
{
    private readonly IPublishedContent _publishedContent;
    private readonly string? _culture;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IResolverContext _resolverContext;

    /// <summary>
    /// Gets the url segment of the media item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the media item.")]
    public string? UrlSegment => _publishedContent?.UrlSegment(_variationContextAccessor, _culture);

    /// <summary>
    /// Gets the url of a media item
    /// </summary>
    [GraphQLDescription("Gets the url of a media item.")]
    public string Url(UrlMode urlMode)
    {
        return _publishedContent.MediaUrl(_culture, urlMode);
    }

    /// <summary>
    /// Gets the name of a media item
    /// </summary>
    [GraphQLDescription("Gets the name of a media item.")]
    public string? Name => _publishedContent?.Name(_variationContextAccessor, _culture);

    /// <summary>
    /// Gets the id of a media item
    /// </summary>
    [GraphQLDescription("Gets the id of a media item.")]
    public int Id => _publishedContent.Id;

    /// <summary>
    /// Gets the key of a media item
    /// </summary>
    [GraphQLDescription("Gets the key of a media item.")]
    public Guid Key => _publishedContent.Key;

    /// <summary>
    /// Gets the properties of the media item
    /// </summary>
    [GraphQLDescription("Gets the properties of the media item.")]
    public TypedProperties Properties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.PublishedContent, _publishedContent);
        return new TypedProperties();
    }

    public MediaPickerItemResponse(IPublishedContent publishedContent, string? culture, IVariationContextAccessor variationContextAccessor, IResolverContext resolverContext)
    {
        _publishedContent = publishedContent;
        _culture = culture;
        _variationContextAccessor = variationContextAccessor;
        _resolverContext = resolverContext;
    }
}
