using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.ContentTypes.Basics.Models;
using Nikcio.UHeadless.ContentTypes.Models;
using Nikcio.UHeadless.Defaults.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Media.Basics.Models;

/// <summary>
/// Represents a Media item
/// </summary>
[GraphQLDescription("Represents a Media item.")]
[Obsolete("Convert to using Defaults.MediaItems.MediaItem or a custom model if you need special properties.")]
public class BasicMedia : BasicMedia<BasicProperty, BasicContentType, BasicMedia>
{
    public BasicMedia(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a Media item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[GraphQLDescription("Represents a Media item.")]
[Obsolete("Convert to using Defaults.MediaItems.MediaItem or a custom model if you need special properties.")]
public class BasicMedia<TProperty> : BasicMedia<TProperty, BasicContentType>
    where TProperty : IProperty
{
    public BasicMedia(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a Media item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TContentType"></typeparam>
[GraphQLDescription("Represents a Media item.")]
[Obsolete("Convert to using Defaults.MediaItems.MediaItem or a custom model if you need special properties.")]
public class BasicMedia<TProperty, TContentType> : BasicMedia<TProperty, TContentType, BasicMedia<TProperty, TContentType>>
    where TProperty : IProperty
    where TContentType : IContentType
{
    public BasicMedia(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a Media item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TContentType"></typeparam>
/// <typeparam name="TMedia"></typeparam>
[GraphQLDescription("Represents a Media item.")]
[Obsolete("Convert to using Defaults.MediaItems.MediaItem or a custom model if you need special properties.")]
public class BasicMedia<TProperty, TContentType, TMedia> : MediaItem
    where TProperty : IProperty
    where TContentType : IContentType
    where TMedia : MediaItemBase
{
    public BasicMedia(CreateCommand command) : base(command)
    {
    }

    /// <summary>
    /// Gets the type of the content item (document, media...)
    /// </summary>
    [GraphQLDescription("Gets the type of the content item (document, media...).")]
    [Obsolete("If you need this add it to your own model")]
    public virtual PublishedItemType? ItemType => PublishedContent?.ItemType;

    /// <summary>
    /// Gets available culture infos
    /// </summary>
    [GraphQLDescription("Gets available culture infos.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual IReadOnlyDictionary<string, PublishedCultureInfo>? Cultures => PublishedContent?.Cultures;

    /// <summary>
    /// Gets the identifier of the user who last updated the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the user who last updated the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? WriterId => PublishedContent?.WriterId;

    /// <summary>
    /// Gets the date that the content was created
    /// </summary>
    [GraphQLDescription("Gets the date that the content was created.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual DateTime? CreateDate => PublishedContent?.CreateDate;

    /// <summary>
    /// Gets the identifier of the user who created the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the user who created the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? CreatorId => PublishedContent?.CreatorId;

    /// <summary>
    /// Gets all the children of the content item, regardless of whether they are available for the current culture
    /// </summary>
    [GraphQLDescription("Gets all the children of the content item, regardless of whether they are available for the current culture.")]
    [UseFiltering]
    [UseSorting]
    [Obsolete("If you need this create your own model with this. I would also recommend not including UseFilering and UseSorting unless you're using it.")]
    public virtual IEnumerable<TMedia?>? ChildrenForAllCultures => PublishedContent?.ChildrenForAllCultures?.Select(child => CreateMediaItem<TMedia>(new CreateCommand()
    {
        PublishedContent = child,
        ResolverContext = ResolverContext
    }, DependencyReflectorFactory));

    /// <summary>
    /// Gets the tree path of the content item
    /// </summary>
    [GraphQLDescription("Gets the tree path of the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual string? Path => PublishedContent?.Path;

    /// <summary>
    /// Gets the tree level of the content item
    /// </summary>
    [GraphQLDescription("Gets the tree level of the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? Level => PublishedContent?.Level;

    /// <summary>
    /// Gets the sort order of the content item
    /// </summary>
    [GraphQLDescription("Gets the sort order of the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? SortOrder => PublishedContent?.SortOrder;

    /// <summary>
    /// Gets the url of the content item
    /// </summary>
    [GraphQLDescription("Gets the url of the content item.")]
    [Obsolete("Use the underlying Url(UrlMode) method instead as this takes a paramenter of the UrlMode")]
    public new string? Url => PublishedContent?.Url(Culture, UrlMode.Default);

    /// <summary>
    /// Gets the absolute url of the content item
    /// </summary>
    [GraphQLDescription("Gets the absolute url of the content item.")]
    [Obsolete("Use the underlying Url(UrlMode) method instead as this takes a paramenter of the UrlMode")]
    public virtual string? AbsoluteUrl => PublishedContent?.Url(Culture, UrlMode.Absolute);

    /// <summary>
    /// Gets the children of the content item that are available for the current cultur
    /// </summary>
    [GraphQLDescription("Gets the children of the content item that are available for the current culture.")]
    [UseFiltering]
    [UseSorting]
    [Obsolete("If you need this create your own model with this. I would also recommend not including UseFilering and UseSorting unless you're using it.")]
    public virtual IEnumerable<TMedia?>? Children => PublishedContent?.Children(Culture)?.Select(child => CreateMediaItem<TMedia>(new CreateCommand()
    {
        PublishedContent = child,
        ResolverContext = ResolverContext
    }, DependencyReflectorFactory));

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content type.")]
    [Obsolete("Use your own model here.")]
    public virtual BasicContentType? ContentType => PublishedContent != null ? new BasicContentType(PublishedContent.ContentType) : default;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the properties of the element.")]
    [UseFiltering]
    [Obsolete("Use typed properties instead.")]
    public new IEnumerable<TProperty?>? Properties => PublishedContent != null ? ResolverContext.Service<IPropertyFactory<TProperty>>().CreateProperties(PublishedContent, Culture, Segment, ResolverContext.GetScopedState<Fallback?>(ContextDataKeys.Fallback)) : default;

    /// <summary>
    /// Gets the named properties of the element using the content types in Umbraco
    /// </summary>
    [GraphQLDescription("Gets the named properties of the element using the content types in Umbraco.")]
    [Obsolete("Use typed properties on the Defaults.ContentItems.ContentItem")]
    public TypedProperties NamedProperties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
