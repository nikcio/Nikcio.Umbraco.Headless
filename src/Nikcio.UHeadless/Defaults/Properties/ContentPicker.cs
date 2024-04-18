using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
public class ContentPicker : ContentPicker<ContentPickerItem>
{
    public ContentPicker(CreateCommand command) : base(command)
    {
    }

    protected override ContentPickerItem CreateContentPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new ContentPickerItem(publishedContent, resolverContext);
    }
}

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
public abstract class ContentPicker<TContentPickerItem> : PropertyValue
    where TContentPickerItem : class
{
    /// <summary>
    /// The content items of the property
    /// </summary>
    /// <value></value>
    protected IEnumerable<IPublishedContent> PublishedContentItems { get; }

    /// <summary>
    /// Gets the content items of a picker
    /// </summary>
    [GraphQLDescription("Gets the content items of a picker.")]
    public List<TContentPickerItem>? Items()
    {
        return PublishedContentItems.Select(publishedContent =>
        {
            return CreateContentPickerItem(publishedContent, ResolverContext);
        }).ToList();
    }

    protected ContentPicker(CreateCommand command) : base(command)
    {
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            PublishedContentItems = new List<IPublishedContent> { publishedContent };
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            PublishedContentItems = publishedContentItems;
        }
        else
        {
            PublishedContentItems = new List<IPublishedContent>();
        }
    }

    /// <summary>
    /// Creates a content picker item
    /// </summary>
    /// <param name="publishedContent"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TContentPickerItem CreateContentPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext);
}

/// <summary>
/// Represents a content picker item
/// </summary>
[GraphQLDescription("Represents a content picker item.")]
public class ContentPickerItem
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
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public string? UrlSegment => PublishedContent?.UrlSegment(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    public string Url(UrlMode urlMode)
    {
        return PublishedContent.Url(Culture, urlMode);
    }

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public string? Name => PublishedContent?.Name(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the id of a content item
    /// </summary>
    [GraphQLDescription("Gets the id of a content item.")]
    public int Id => PublishedContent.Id;

    /// <summary>
    /// Gets the key of a content item
    /// </summary>
    [GraphQLDescription("Gets the key of a content item.")]
    public Guid Key => PublishedContent.Key;

    /// <summary>
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }

    public ContentPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        PublishedContent = publishedContent;
        ResolverContext = resolverContext;
        Culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        VariationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
