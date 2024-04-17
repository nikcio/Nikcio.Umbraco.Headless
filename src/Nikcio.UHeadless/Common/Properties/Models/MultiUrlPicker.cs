using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents a multi url picker
/// </summary>
[GraphQLDescription("Represents a multi url picker.")]
public class MultiUrlPicker : PropertyValue
{
    private readonly IEnumerable<Link> _publishedContentItemsLinks;
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    /// <summary>
    /// Gets the content items of a picker
    /// </summary>
    [GraphQLDescription("Gets the content items of a picker.")]
    public List<MultiUrlPickerItem> ContentItems => _publishedContentItemsLinks.Select(link =>
    {
        if (!_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot) || publishedSnapshot == null)
        {
            ResolverContext.Service<ILogger<MultiUrlPicker>>().LogError("Could not get published snapshot.");
            return null;
        }

        if (link.Udi == null)
        {
            ResolverContext.Service<ILogger<MultiUrlPicker>>().LogWarning("Could not get Udi in multi url link.");
            return null;
        }

        IPublishedContent? publishedContent = publishedSnapshot.Content?.GetById(IsPreview, link.Udi);

        if (publishedContent == null)
        {
            ResolverContext.Service<ILogger<MultiUrlPicker>>().LogWarning("Could not get published content.");
            return null;
        }

        return new MultiUrlPickerItem(publishedContent, ResolverContext);
    }).OfType<MultiUrlPickerItem>().ToList();

    public MultiUrlPicker(CreateCommand command) : base(command)
    {
        _publishedSnapshotAccessor = ResolverContext.Service<IPublishedSnapshotAccessor>();

        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is Link publishedContent)
        {
            _publishedContentItemsLinks = new List<Link> { publishedContent };
        }
        else if (publishedContentItemsAsObject is IEnumerable<Link> publishedContentItems)
        {
            _publishedContentItemsLinks = publishedContentItems;
        }
        else
        {
            _publishedContentItemsLinks = new List<Link>();
        }
    }
}

/// <summary>
/// Represents a content item
/// </summary>
[GraphQLDescription("Represents a content item.")]
public class MultiUrlPickerItem
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

    public MultiUrlPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        _publishedContent = publishedContent;
        _resolverContext = resolverContext;
        _culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        _variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
