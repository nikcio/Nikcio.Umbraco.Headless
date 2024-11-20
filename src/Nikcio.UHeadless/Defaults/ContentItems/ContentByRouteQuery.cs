using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Authorization;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.ContentItems;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByRouteQuery : ContentByRouteQuery<ContentItem>
{
    protected override async Task<ContentItem?> CreateContentItemFromRouteAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        IPublishedRequest contentRequest = await GetContentRequestAsync(resolverContext, route, baseUrl).ConfigureAwait(false);

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        ContentItem? contentItem;
        switch (contentRequest.GetRouteResult())
        {
            case UmbracoRouteResult.Success:
                contentItem = contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
                {
                    PublishedContent = contentRequest.PublishedContent,
                    ResolverContext = resolverContext,
                    Redirect = null,
                    StatusCode = contentRequest.PublishedContent == null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
                });
                break;

            case UmbracoRouteResult.Redirect:
                bool isRedirectPermanent = contentRequest.IsRedirectPermanent();
                contentItem = contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
                {
                    PublishedContent = contentRequest.PublishedContent,
                    ResolverContext = resolverContext,
                    Redirect = new()
                    {
                        IsPermanent = isRedirectPermanent,
                        RedirectUrl = contentRequest.RedirectUrl,
                    },
                    StatusCode = isRedirectPermanent ? StatusCodes.Status308PermanentRedirect : StatusCodes.Status307TemporaryRedirect,
                });
                break;

            case UmbracoRouteResult.NotFound:
                contentItem = contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
                {
                    PublishedContent = contentRequest.PublishedContent,
                    ResolverContext = resolverContext,
                    Redirect = null,
                    StatusCode = StatusCodes.Status404NotFound,
                });
                break;

            default:
                throw new InvalidOperationException("The route result is not valid");
        }

        return contentItem;
    }

    protected override ContentItem? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = publishedContent == null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
        });
    }
}

/// <summary>
/// Implements the <see cref="ContentByRouteAsync" /> query
/// </summary>
public abstract class ContentByRouteQuery<TContentItem> : IGraphQLQuery
    where TContentItem : ContentItemBase
{
    public const string PolicyName = "ContentByRouteQuery";

    public const string ClaimValue = "content.by.route.query";

    [GraphQLIgnore]
    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorizationBuilder().AddPolicy(PolicyName, policy =>
        {
            if (options.DisableAuthorization)
            {
                policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
                return;
            }

            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadless);

            policy.RequireAuthenticatedUser();

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalContentRead);
        });

        AvailableClaimValue availableClaimValue = new()
        {
            Name = DefaultClaims.UHeadlessScope,
            Values = [ClaimValue, DefaultClaimValues.GlobalContentRead]
        };
        AuthorizationTokenProvider.AddAvailableClaimValue(ClaimValueGroups.Content, availableClaimValue);
    }

    /// <summary>
    /// Gets a content item by an absolute route
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLName("contentByRoute")]
    [GraphQLDescription("Gets a content item by a route.")]
    public virtual async Task<TContentItem?> ContentByRouteAsync(
        IResolverContext resolverContext,
        [GraphQLDescription("The route to fetch. Example '/da/frontpage/'.")] string route,
        [GraphQLDescription("The base url for the request. Example: 'https://localhost:4000'. Default is the current domain")] string baseUrl = "",
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(route);

        inContext ??= new QueryContext();
        if (!inContext.Initialize(resolverContext))
        {
            throw new InvalidOperationException("The context could not be initialized");
        }

        TContentItem? contentItem = await CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);

        if (contentItem != null)
        {
            return contentItem;
        }

        IContentItemRepository<TContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<TContentItem>>();
        IDocumentUrlService documentUrlService = resolverContext.Service<IDocumentUrlService>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        Guid? contentKey = documentUrlService.GetDocumentKeyByRoute(route, inContext.Culture, null, inContext.IncludePreview.Value);

        if (contentKey == null)
        {
            return CreateContentItem(null, contentItemRepository, resolverContext);
        }

        IPublishedContent? publishedContent = contentCache.GetById(inContext.IncludePreview.Value, contentKey.Value);

        return CreateContentItem(publishedContent, contentItemRepository, resolverContext);
    }

    /// <summary>
    /// Creates the content item from the published content.
    /// </summary>
    /// <remarks>
    /// This runs if the content item is not found from the route via the <see cref="CreateContentItemFromRouteAsync(IResolverContext, string, string)"/>.
    /// </remarks>
    protected abstract TContentItem? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<TContentItem> contentItemRepository, IResolverContext resolverContext);

    /// <summary>
    /// Resolves the content item from the route.
    /// </summary>
    /// <remarks>
    /// This runs first and if the content item is not found, it will run the <see cref="CreateContentItem(IPublishedContent?, IContentItemRepository{TContentItem}, IResolverContext)"/> to create the content item.
    /// </remarks>
    protected abstract Task<TContentItem?> CreateContentItemFromRouteAsync(IResolverContext resolverContext, string route, string baseUrl);

    /// <summary>
    /// Gets the <see cref="IPublishedRequest"/> from the <see cref="IPublishedRouter"/>.
    /// </summary>
    protected async Task<IPublishedRequest> GetContentRequestAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IPublishedRouter publishedRouter = resolverContext.Service<IPublishedRouter>();
        IHttpContextAccessor? httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();

        baseUrl = SetBaseUrl(httpContextAccessor, baseUrl);

        var uri = new Uri($"{baseUrl.TrimEnd('/')}{route}");

        IPublishedRequestBuilder builder = await publishedRouter.CreateRequestAsync(uri).ConfigureAwait(false);
        IPublishedRequest request = await publishedRouter.RouteRequestAsync(builder, new RouteRequestOptions(RouteDirection.Inbound)).ConfigureAwait(false);

        return request;
    }

    private static string SetBaseUrl(IHttpContextAccessor httpContextAccessor, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            {
                throw new HttpRequestException("HttpContext could not be found");
            }

            baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Host}";

            if (httpContextAccessor.HttpContext.Request.Host.Port is not 80 and not 443)
            {
                baseUrl += $":{httpContextAccessor.HttpContext.Request.Host.Port}";
            }
        }

        return baseUrl;
    }
}
