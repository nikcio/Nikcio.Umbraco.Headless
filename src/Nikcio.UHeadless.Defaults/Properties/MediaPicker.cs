using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
public class MediaPicker : PropertyValue
{
    private readonly IEnumerable<IPublishedContent> _publishedMediaItems;

    /// <summary>
    /// Gets the media items of a picker
    /// </summary>
    [GraphQLDescription("Gets the media items of a picker.")]
    public List<MediaPickerItem> MediaItems => _publishedMediaItems.Select(publishedMedia =>
    {
        return new MediaPickerItem(publishedMedia, ResolverContext);
    }).ToList();

    public MediaPicker(CreateCommand command) : base(command)
    {
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            _publishedMediaItems = new List<IPublishedContent> { publishedContent };
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            _publishedMediaItems = publishedContentItems;
        }
        else
        {
            _publishedMediaItems = new List<IPublishedContent>();
        }
    }
}

/// <summary>
/// Represents a media item
/// </summary>
[GraphQLDescription("Represents a media item.")]
public class MediaPickerItem
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

    public MediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        _publishedContent = publishedContent;
        _resolverContext = resolverContext;
        _culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        _variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
