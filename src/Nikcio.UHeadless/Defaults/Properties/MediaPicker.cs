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
public class MediaPicker : MediaPicker<MediaPickerItem>
{
    public MediaPicker(CreateCommand command) : base(command)
    {
    }

    protected override MediaPickerItem CreateMediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new MediaPickerItem(publishedContent, resolverContext);
    }
}

/// <summary>
/// Represents a media picker item
/// </summary>
[GraphQLDescription("Represents a media picker item.")]
public abstract class MediaPicker<TMediaPickerItem> : PropertyValue
    where TMediaPickerItem : class
{
    protected IEnumerable<IPublishedContent> PublishedMediaItems { get; }

    /// <summary>
    /// Gets the media items of a picker
    /// </summary>
    [GraphQLDescription("Gets the media items of a picker.")]
    public List<TMediaPickerItem> MediaItems()
    {
        return PublishedMediaItems.Select(publishedMedia =>
        {
            return CreateMediaPickerItem(publishedMedia, ResolverContext);
        }).ToList();
    }

    protected MediaPicker(CreateCommand command) : base(command)
    {
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            PublishedMediaItems = new List<IPublishedContent> { publishedContent };
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            PublishedMediaItems = publishedContentItems;
        }
        else
        {
            PublishedMediaItems = new List<IPublishedContent>();
        }
    }

    /// <summary>
    /// Creates a media picker item
    /// </summary>
    /// <param name="publishedContent"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TMediaPickerItem CreateMediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext);
}

/// <summary>
/// Represents a media item
/// </summary>
[GraphQLDescription("Represents a media item.")]
public class MediaPickerItem
{
    /// <summary>
    /// The published content
    /// </summary>
    /// <value></value>
    protected IPublishedContent PublishedContent { get; }

    /// <summary>
    /// The culture of the query
    /// </summary>
    /// <value></value>
    protected string? Culture { get; }

    /// <summary>
    /// The variation context accessor
    /// </summary>
    /// <value></value>
    protected IVariationContextAccessor VariationContextAccessor { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    /// <value></value>
    protected IResolverContext ResolverContext { get; }

    /// <summary>
    /// Gets the url segment of the media item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the media item.")]
    public string? UrlSegment => PublishedContent?.UrlSegment(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of a media item
    /// </summary>
    [GraphQLDescription("Gets the url of a media item.")]
    public string Url(UrlMode urlMode)
    {
        return PublishedContent.MediaUrl(Culture, urlMode);
    }

    /// <summary>
    /// Gets the name of a media item
    /// </summary>
    [GraphQLDescription("Gets the name of a media item.")]
    public string? Name => PublishedContent?.Name(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the id of a media item
    /// </summary>
    [GraphQLDescription("Gets the id of a media item.")]
    public int Id => PublishedContent.Id;

    /// <summary>
    /// Gets the key of a media item
    /// </summary>
    [GraphQLDescription("Gets the key of a media item.")]
    public Guid Key => PublishedContent.Key;

    /// <summary>
    /// Gets the properties of the media item
    /// </summary>
    [GraphQLDescription("Gets the properties of the media item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }

    public MediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        PublishedContent = publishedContent;
        ResolverContext = resolverContext;
        Culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        VariationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
