using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Properties;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

public partial class ContentItem : ContentItemBase
{
    protected IVariationContextAccessor VariationContextAccessor { get; }

    protected IDependencyReflectorFactory DependencyReflectorFactory { get; }

    /// <summary>
    /// The document url service
    /// </summary>
    protected IDocumentUrlService DocumentUrlService { get; }

    protected IDocumentNavigationQueryService DocumentNavigationQueryService { get; }

    protected IPublishedContentCache PublishedContentCache { get; }

    public ContentItem(CreateCommand command) : base(command)
    {
        ArgumentNullException.ThrowIfNull(command);

        VariationContextAccessor = command.ResolverContext.Service<IVariationContextAccessor>();
        DependencyReflectorFactory = command.ResolverContext.Service<IDependencyReflectorFactory>();
        DocumentUrlService = command.ResolverContext.Service<IDocumentUrlService>();
        DocumentNavigationQueryService = command.ResolverContext.Service<IDocumentNavigationQueryService>();
        PublishedContentCache = command.ResolverContext.Service<IPublishedContentCache>();

        StatusCode = command.StatusCode;
        Redirect = command.Redirect == null ? null : new RedirectInfo()
        {
            IsPermanent = command.Redirect.IsPermanent,
            RedirectUrl = command.Redirect.RedirectUrl
        };
    }

    /// <summary>
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public string? UrlSegment(IResolverContext resolverContext)
    {
        return PublishedContent != null ? DocumentUrlService.GetUrlSegment(PublishedContent.Key, resolverContext.Culture() ?? VariationContextAccessor.VariationContext?.Culture ?? "*", PublishedContent.IsPublished(resolverContext.Culture())) : null;
    }

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    public string? Url(IResolverContext resolverContext, UrlMode urlMode)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        IPublishedUrlProvider publishedUrlProvider = resolverContext.Service<IPublishedUrlProvider>();

        if (PublishedContent == null)
        {
            return default;
        }

        return publishedUrlProvider.GetUrl(PublishedContent, urlMode, resolverContext.Culture(), new Uri(resolverContext.BaseUrl()));
    }

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public string? Name(IResolverContext resolverContext)
    {
        return PublishedContent?.Name(VariationContextAccessor, resolverContext.Culture());
    }

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
    /// Gets the identifier of the template to use to render the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the template to use to render the content item.")]
    public int? TemplateId => PublishedContent?.TemplateId;

    /// <summary>
    /// Gets the date the content item was last updated
    /// </summary>
    [GraphQLDescription("Gets the date the content item was last updated.")]
    public DateTime? UpdateDate => PublishedContent?.UpdateDate;

    /// <summary>
    /// Gets the parent of the content item
    /// </summary>
    [GraphQLDescription("Gets the parent of the content item.")]
    public ContentItem? Parent(IResolverContext resolverContext)
    {
        if (PublishedContent == null)
        {
            return default;
        }

        if (!DocumentNavigationQueryService.TryGetParentKey(PublishedContent.Key, out Guid? parentKey) || parentKey == null)
        {
            return default;
        }

        IPublishedContent? parent = PublishedContentCache.GetById(resolverContext.IncludePreview(), parentKey.Value);

        if (parent == null)
        {
            return default;
        }

        return CreateContentItem<ContentItem>(new CreateCommand()
        {
            PublishedContent = parent,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = StatusCodes.Status200OK,
        }, DependencyReflectorFactory);
    }

    /// <summary>
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties(IResolverContext resolverContext)
    {
        resolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }

    public int StatusCode { get; }

    /// <summary>
    /// Gets the redirect information for the content item
    /// </summary>
    [GraphQLDescription("Gets the redirect information for the content item.")]
    public RedirectInfo? Redirect { get; }

    public class RedirectInfo
    {
        public required string? RedirectUrl { get; init; }

        public required bool IsPermanent { get; init; }
    }
}
