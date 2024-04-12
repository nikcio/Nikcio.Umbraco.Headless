using HotChocolate;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.Reflection;
using Nikcio.UHeadless.Media;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Examples.Docs.Media;

public class MediaItem : MemberItem
{
    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public MediaItem(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory) : base(command)
    {
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);
        ArgumentNullException.ThrowIfNull(command);

        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <summary>
    /// Gets the url segment of the media item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the media item.")]
    public string? UrlSegment => PublishedContent?.UrlSegment;

    /// <summary>
    /// Gets the url of a media item
    /// </summary>
    [GraphQLDescription("Gets the url of a media item.")]
    public string? Url(UrlMode urlMode)
    {
        return PublishedContent?.MediaUrl(mode: urlMode);
    }

    /// <summary>
    /// Gets the name of a media item
    /// </summary>
    [GraphQLDescription("Gets the name of a media item.")]
    public string? Name => PublishedContent?.Name;

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
    }, _dependencyReflectorFactory) : default;

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
