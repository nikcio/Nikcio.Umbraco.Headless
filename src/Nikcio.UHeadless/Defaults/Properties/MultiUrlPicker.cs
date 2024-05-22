using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a multi url picker
/// </summary>
[GraphQLDescription("Represents a multi url picker.")]
public class MultiUrlPicker : MultiUrlPicker<MultiUrlPickerItem>
{
    public MultiUrlPicker(CreateCommand command) : base(command)
    {
    }

    protected override MultiUrlPickerItem CreateMultiUrlPickerItem(IPublishedContent? publishedContent, Link link, IResolverContext resolverContext)
    {
        return new MultiUrlPickerItem(publishedContent, link, resolverContext);
    }
}

/// <summary>
/// Represents a multi url picker
/// </summary>
[GraphQLDescription("Represents a multi url picker.")]
public abstract class MultiUrlPicker<TMultiUrlPickerItem> : PropertyValue
    where TMultiUrlPickerItem : class
{
    /// <summary>
    /// The published content items
    /// </summary>
    /// <value></value>
    protected List<Link> PublishedContentItemsLinks { get; }

    /// <summary>
    /// The published snapshot accessor
    /// </summary>
    /// <value></value>
    protected IPublishedSnapshotAccessor PublishedSnapshotAccessor { get; }

    /// <summary>
    /// Gets the links of the picker
    /// </summary>
    [GraphQLDescription("Gets the links of the picker.")]
    public List<TMultiUrlPickerItem> Links(IResolverContext resolverContext)
    {
        return PublishedContentItemsLinks.Select(link =>
        {
            if (!PublishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot) || publishedSnapshot == null)
            {
                resolverContext.Service<ILogger<MultiUrlPicker>>().LogError("Could not get published snapshot.");
                return null;
            }

            if (link.Udi == null)
            {
                return CreateMultiUrlPickerItem(null, link, resolverContext);
            }

            IPublishedContent? publishedContent = publishedSnapshot.Content?.GetById(resolverContext.IncludePreview(), link.Udi);

            return CreateMultiUrlPickerItem(publishedContent, link, resolverContext);
        }).OfType<TMultiUrlPickerItem>().ToList();
    }

    protected MultiUrlPicker(CreateCommand command) : base(command)
    {
        PublishedSnapshotAccessor = command.ResolverContext.Service<IPublishedSnapshotAccessor>();

        IResolverContext resolverContext = command.ResolverContext;
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, resolverContext.Culture(), resolverContext.Segment(), resolverContext.Fallback());

        if (publishedContentItemsAsObject is Link publishedContent)
        {
            PublishedContentItemsLinks = [publishedContent];
        }
        else if (publishedContentItemsAsObject is IEnumerable<Link> publishedContentItems)
        {
            PublishedContentItemsLinks = publishedContentItems.ToList();
        }
        else
        {
            PublishedContentItemsLinks = [];
        }
    }

    /// <summary>
    /// Creates a multi url picker item
    /// </summary>
    /// <param name="publishedContent"></param>
    /// <param name="link"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TMultiUrlPickerItem CreateMultiUrlPickerItem(IPublishedContent? publishedContent, Link link, IResolverContext resolverContext);
}

/// <summary>
/// Represents a content item
/// </summary>
[GraphQLDescription("Represents a content item.")]
public class MultiUrlPickerItem
{
    /// <summary>
    /// The published content
    /// </summary>
    /// <value></value>
    protected IPublishedContent? PublishedContent { get; }

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
    /// The link
    /// </summary>
    protected Link Link { get; }

    /// <summary>
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public string? UrlSegment => PublishedContent?.UrlSegment(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item. If the link isn't to a content item or media item then the UrlMode doesn't affect the url.")]
    public string Url(IResolverContext resolverContext, UrlMode urlMode)
    {
        return PublishedContent?.Url(resolverContext.Service<IPublishedUrlProvider>(), Culture, urlMode) ?? Link.Url ?? string.Empty;
    }

    /// <summary>
    /// Gets the target of the link
    /// </summary>
    [GraphQLDescription("Gets the target of the link.")]
    public string? Target => Link.Target;

    /// <summary>
    /// Gets the type of the link
    /// </summary>
    [GraphQLDescription("Gets the type of the link.")]
    public LinkType Type => Link.Type;

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public string? Name => PublishedContent?.Name(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the id of a content item
    /// </summary>
    [GraphQLDescription("Gets the id of a content item.")]
    public int? Id => PublishedContent?.Id;

    /// <summary>
    /// Gets the key of a content item
    /// </summary>
    [GraphQLDescription("Gets the key of a content item.")]
    public Guid? Key => PublishedContent?.Key;

    /// <summary>
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }

    public MultiUrlPickerItem(IPublishedContent? publishedContent, Link link, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        PublishedContent = publishedContent;
        ResolverContext = resolverContext;
        Link = link;
        Culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        VariationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
