using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Factories;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.TypeModules;
using Nikcio.UHeadless.ContentTypes.Basics.Models;
using Nikcio.UHeadless.ContentTypes.Factories;
using Nikcio.UHeadless.ContentTypes.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Content.Basics.Models;

/// <summary>
/// Represents a content item
/// </summary>
[GraphQLDescription("Represents a content item.")]
public class BasicContent : BasicContent<BasicProperty, BasicContentType, BasicContentRedirect, BasicContent>
{
    /// <inheritdoc/>
    public BasicContent(CreateContent createContent, IPropertyFactory<BasicProperty> propertyFactory, IContentTypeFactory<BasicContentType> contentTypeFactory, IContentFactory<BasicContent> contentFactory, IVariationContextAccessor variationContextAccessor) : base(createContent, propertyFactory, contentTypeFactory, contentFactory, variationContextAccessor)
    {
    }
}

/// <summary>
/// Represents a content item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[GraphQLDescription("Represents a content item.")]
public class BasicContent<TProperty> : BasicContent<TProperty, BasicContentType>
    where TProperty : IProperty
{
    /// <inheritdoc/>
    public BasicContent(CreateContent createContent, IPropertyFactory<TProperty> propertyFactory, IContentTypeFactory<BasicContentType> contentTypeFactory, IContentFactory<BasicContent<TProperty, BasicContentType, BasicContentRedirect>> contentFactory, IVariationContextAccessor variationContextAccessor) : base(createContent, propertyFactory, contentTypeFactory, contentFactory, variationContextAccessor)
    {
    }
}

/// <summary>
/// Represents a content item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TContentType"></typeparam>
[GraphQLDescription("Represents a content item.")]
public class BasicContent<TProperty, TContentType> : BasicContent<TProperty, TContentType, BasicContentRedirect>
    where TProperty : IProperty
    where TContentType : IContentType
{
    /// <inheritdoc/>
    public BasicContent(CreateContent createContent, IPropertyFactory<TProperty> propertyFactory, IContentTypeFactory<TContentType> contentTypeFactory, IContentFactory<BasicContent<TProperty, TContentType, BasicContentRedirect>> contentFactory, IVariationContextAccessor variationContextAccessor) : base(createContent, propertyFactory, contentTypeFactory, contentFactory, variationContextAccessor)
    {
    }
}

/// <summary>
/// Represents a content item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TContentType"></typeparam>
/// <typeparam name="TContentRedirect"></typeparam>
[GraphQLDescription("Represents a content item.")]
public class BasicContent<TProperty, TContentType, TContentRedirect> : BasicContent<TProperty, TContentType, TContentRedirect, BasicContent<TProperty, TContentType, TContentRedirect>>
    where TProperty : IProperty
    where TContentType : IContentType
    where TContentRedirect : IContentRedirect
{
    /// <inheritdoc/>
    public BasicContent(CreateContent createContent, IPropertyFactory<TProperty> propertyFactory, IContentTypeFactory<TContentType> contentTypeFactory, IContentFactory<BasicContent<TProperty, TContentType, TContentRedirect>> contentFactory, IVariationContextAccessor variationContextAccessor) : base(createContent, propertyFactory, contentTypeFactory, contentFactory, variationContextAccessor)
    {
    }
}

/// <summary>
/// Represents a content item
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TContentType"></typeparam>
/// <typeparam name="TContentRedirect"></typeparam>
/// <typeparam name="TContent"></typeparam>
[GraphQLDescription("Represents a content item.")]
public class BasicContent<TProperty, TContentType, TContentRedirect, TContent> : Content<TProperty>
    where TProperty : IProperty
    where TContentType : IContentType
    where TContentRedirect : IContentRedirect
    where TContent : IContent
{
    /// <inheritdoc/>
    public BasicContent(CreateContent createContent, IPropertyFactory<TProperty> propertyFactory, IContentTypeFactory<TContentType> contentTypeFactory, IContentFactory<TContent> contentFactory, IVariationContextAccessor variationContextAccessor) : base(createContent, propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(variationContextAccessor, nameof(variationContextAccessor));

        ContentFactory = contentFactory;
        ContentTypeFactory = contentTypeFactory;
        variationContextAccessor.VariationContext = new VariationContext(Culture);
        VariationContextAccessor = variationContextAccessor;
    }

    /// <summary>
    /// Gets the identifier of the template to use to render the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the template to use to render the content item.")]
    public virtual int? TemplateId => Content?.TemplateId;

    /// <summary>
    /// Gets the parent of the content item
    /// </summary>
    [GraphQLDescription("Gets the parent of the content item.")]
    public virtual TContent? Parent => Content?.Parent != null ? ContentFactory.CreateContent(Content.Parent, Culture, Segment, Fallback) : default;

    /// <summary>
    /// Gets the type of the content item (document, media...)
    /// </summary>
    [GraphQLDescription("Gets the type of the content item (document, media...).")]
    public virtual PublishedItemType? ItemType => Content?.ItemType;

    /// <summary>
    /// Gets available culture infos
    /// </summary>
    [GraphQLDescription("Gets available culture infos.")]
    public virtual IReadOnlyDictionary<string, PublishedCultureInfo>? Cultures => Content?.Cultures;

    /// <summary>
    /// Gets the date the content item was last updated
    /// </summary>
    [GraphQLDescription("Gets the date the content item was last updated.")]
    public virtual DateTime? UpdateDate => Content?.UpdateDate;

    /// <summary>
    /// Gets the identifier of the user who last updated the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the user who last updated the content item.")]
    public virtual int? WriterId => Content?.WriterId;

    /// <summary>
    /// Gets the date that the content was created
    /// </summary>
    [GraphQLDescription("Gets the date that the content was created.")]
    public virtual DateTime? CreateDate => Content?.CreateDate;

    /// <summary>
    /// Gets the identifier of the user who created the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the user who created the content item.")]
    public virtual int? CreatorId => Content?.CreatorId;

    /// <summary>
    /// Gets all the children of the content item, regardless of whether they are available for the current culture
    /// </summary>
    [GraphQLDescription("Gets all the children of the content item, regardless of whether they are available for the current culture.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TContent?>? ChildrenForAllCultures => Content?.ChildrenForAllCultures?.Select(child => ContentFactory.CreateContent(child, Culture, Segment, Fallback));

    /// <summary>
    /// Gets the tree path of the content item
    /// </summary>
    [GraphQLDescription("Gets the tree path of the content item.")]
    public virtual string? Path => Content?.Path;

    /// <summary>
    /// Gets the tree level of the content item
    /// </summary>
    [GraphQLDescription("Gets the tree level of the content item.")]
    public virtual int? Level => Content?.Level;

    /// <summary>
    /// Gets the sort order of the content item
    /// </summary>
    [GraphQLDescription("Gets the sort order of the content item.")]
    public virtual int? SortOrder => Content?.SortOrder;

    /// <summary>
    /// Gets the URL segment of the content item for the current culture
    /// </summary>
    [GraphQLDescription("Gets the URL segment of the content item for the current culture.")]
    public virtual string? UrlSegment => Content?.UrlSegment(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of the content item
    /// </summary>
    [GraphQLDescription("Gets the url of the content item.")]
    public virtual string? Url => Content?.Url(Culture, UrlMode.Default);

    /// <summary>
    /// Gets the absolute url of the content item
    /// </summary>
    [GraphQLDescription("Gets the absolute url of the content item.")]
    public virtual string? AbsoluteUrl => Content?.Url(Culture, UrlMode.Absolute);

    /// <summary>
    /// Gets the name of the content item for the current culture
    /// </summary>
    [GraphQLDescription("Gets the name of the content item for the current culture.")]
    public virtual string? Name => Content?.Name(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the unique identifier of the content item
    /// </summary>
    [GraphQLDescription("Gets the unique identifier of the content item.")]
    public virtual int? Id => Content?.Id;

    /// <summary>
    /// Gets the children of the content item that are available for the current cultur
    /// </summary>
    [GraphQLDescription("Gets the children of the content item that are available for the current culture.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TContent?>? Children => Content?.Children(VariationContextAccessor, Culture)?.Select(child => ContentFactory.CreateContent(child, Culture, Segment, Fallback));

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content type.")]
    public virtual TContentType? ContentType => Content != null ? ContentTypeFactory.CreateContentType(Content.ContentType) : default;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the unique key of the element.")]
    public virtual Guid? Key => Content?.Key;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the properties of the element.")]
    [UseFiltering]
    public virtual IEnumerable<TProperty?>? Properties => Content != null ? PropertyFactory.CreateProperties(Content, Culture, Segment, Fallback) : default;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the redirect information.")]
    public new virtual TContentRedirect? Redirect { get; set; }

    /// <summary>
    /// Gets the named properties of the element using the content types in Umbraco
    /// </summary>
    [GraphQLDescription("Gets the named properties of the element using the content types in Umbraco.")]
    public virtual INamedContentProperties NamedProperties(IResolverContext context)
    {
        context.SetScopedState(ContentTypeModule.ElementScopedStateKey, this);

        return new NamedContentProperties();
    }

    /// <summary>
    /// The content factory
    /// </summary>
    protected virtual IContentFactory<TContent> ContentFactory { get; }

    /// <summary>
    /// The content type factory
    /// </summary>
    protected virtual IContentTypeFactory<TContentType> ContentTypeFactory { get; }

    /// <summary>
    /// The variation context accessor
    /// </summary>
    /// <value></value>
    protected virtual IVariationContextAccessor VariationContextAccessor { get; }
}
