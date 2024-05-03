using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.Properties;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.MediaItems;

public class MediaItem : MediaItemBase
{
    protected IVariationContextAccessor VariationContextAccessor { get; }

    protected IDependencyReflectorFactory DependencyReflectorFactory { get; }

    protected IPublishedUrlProvider PublishedUrlProvider { get; }

    public MediaItem(CreateCommand command) : base(command)
    {
        ArgumentNullException.ThrowIfNull(command);

        VariationContextAccessor = ResolverContext.Service<IVariationContextAccessor>();
        DependencyReflectorFactory = ResolverContext.Service<IDependencyReflectorFactory>();
        PublishedUrlProvider = ResolverContext.Service<IPublishedUrlProvider>();
    }

    /// <summary>
    /// Gets the url segment of the media item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the media item.")]
    public string? UrlSegment => PublishedContent?.UrlSegment(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of a media item
    /// </summary>
    [GraphQLDescription("Gets the url of a media item.")]
    public string? Url(UrlMode urlMode, string propertyAlias = "umbracoFile")
    {
        return PublishedContent?.MediaUrl(PublishedUrlProvider, Culture, urlMode, propertyAlias);
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
    public int? Id => PublishedContent?.Id;

    /// <summary>
    /// Gets the key of a media item
    /// </summary>
    [GraphQLDescription("Gets the key of a media item.")]
    public Guid? Key => PublishedContent?.Key;

    /// <summary>
    /// Gets the identifier of the template to use to render the media item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the template to use to render the media item.")]
    public int? TemplateId => PublishedContent?.TemplateId;

    /// <summary>
    /// Gets the date the media item was last updated
    /// </summary>
    [GraphQLDescription("Gets the date the media item was last updated.")]
    public DateTime? UpdateDate => PublishedContent?.UpdateDate;

    /// <summary>
    /// Gets the parent of the media item
    /// </summary>
    [GraphQLDescription("Gets the parent of the media item.")]
    public MediaItem? Parent => PublishedContent?.Parent != null ? CreateMediaItem<MediaItem>(new CreateCommand()
    {
        PublishedContent = PublishedContent.Parent,
        ResolverContext = ResolverContext,
    }, DependencyReflectorFactory) : default;

    /// <summary>
    /// Gets the properties of the media item
    /// </summary>
    [GraphQLDescription("Gets the properties of the media item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
