using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.ContentTypes.Basics.Models;
using Nikcio.UHeadless.ContentTypes.Models;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Content.Basics.Models;

/// <summary>
/// Represents a content item
/// </summary>
[GraphQLDescription("Represents a content item.")]
[Obsolete("Convert to using Defaults.ContentItems.ContentItem or a custom model if you need special properties.")]
public class BasicContent : BasicContent<BasicProperty, BasicContentType, BasicContent>
{
    public BasicContent(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a content item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[GraphQLDescription("Represents a content item.")]
[Obsolete("Convert to using Defaults.ContentItems.ContentItem or a custom model if you need special properties.")]
public class BasicContent<TProperty> : BasicContent<TProperty, BasicContentType, BasicContent>
    where TProperty : IProperty
{
    public BasicContent(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a content item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TContentType"></typeparam>
/// <typeparam name="TContent"></typeparam>
[GraphQLDescription("Represents a content item.")]
[Obsolete("Convert to using Defaults.ContentItems.ContentItem or a custom model if you need special properties.")]
public class BasicContent<TProperty, TContentType, TContent> : ContentItem
    where TProperty : IProperty
    where TContentType : IContentType
    where TContent : ContentItemBase
{
    public BasicContent(CreateCommand command) : base(command)
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
    [Obsolete("If you need this create your own model with this. I would also recommend not including UseFilering and UseSorting unless you're using it.")]
    public virtual IEnumerable<TContent?>? ChildrenForAllCultures(IResolverContext resolverContext)
    {
        return PublishedContent?.ChildrenForAllCultures?.Select(child => CreateContentItem<TContent>(new CreateCommand()
        {
            PublishedContent = child,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200,
        }, DependencyReflectorFactory));
    }

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
    public string? Url(IResolverContext resolverContext)
    {
        return PublishedContent?.Url(resolverContext.Culture(), UrlMode.Default);
    }

    /// <summary>
    /// Gets the absolute url of the content item
    /// </summary>
    [GraphQLDescription("Gets the absolute url of the content item.")]
    [Obsolete("Use the underlying Url(UrlMode) method instead as this takes a paramenter of the UrlMode")]
    public virtual string? AbsoluteUrl(IResolverContext resolverContext)
    {
        return PublishedContent?.Url(resolverContext.Culture(), UrlMode.Absolute);
    }

    /// <summary>
    /// Gets the children of the content item that are available for the current cultur
    /// </summary>
    [GraphQLDescription("Gets the children of the content item that are available for the current culture.")]
    [Obsolete("If you need this create your own model with this. I would also recommend not including UseFilering and UseSorting unless you're using it.")]
    public virtual IEnumerable<TContent?>? Children(IResolverContext resolverContext)
    {
        return PublishedContent?.Children(VariationContextAccessor, resolverContext.Culture())?.Select(child => CreateContentItem<TContent>(new CreateCommand()
        {
            PublishedContent = child,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        }, DependencyReflectorFactory));
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content type.")]
    [Obsolete("Use your own model here.")]
    public virtual BasicContentType? ContentType => PublishedContent != null ? new BasicContentType(PublishedContent.ContentType) : default;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the properties of the element.")]
    [Obsolete("Use typed properties instead.")]
    public new IEnumerable<TProperty?>? Properties(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        return PublishedContent != null
            ? resolverContext.Service<IPropertyFactory<TProperty>>().CreateProperties(
                PublishedContent,
                resolverContext.Culture(),
                resolverContext.Segment(),
                resolverContext.Fallback())
            : default;
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the redirect information.")]
    [Obsolete("Convert to using the Redirect method on Defaults.ContentItems.ContentItem")]
    public new virtual object? Redirect { get; set; }

    /// <summary>
    /// Gets the named properties of the element using the content types in Umbraco
    /// </summary>
    [GraphQLDescription("Gets the named properties of the element using the content types in Umbraco.")]
    [Obsolete("Use typed properties on the Defaults.ContentItems.ContentItem")]
    public TypedProperties NamedProperties(IResolverContext resolverContext)
    {
        resolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
