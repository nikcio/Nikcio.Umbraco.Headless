using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentByRouteAsync" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByRouteQuery : IGraphQLQuery
{
    public const string PolicyName = "ContentByRouteQuery";

    public const string ClaimValue = "content.by.route.query";

    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorization(configure =>
        {
            configure.AddPolicy(PolicyName, policy =>
            {
                if (options.DisableAuthorization)
                {
                    policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
                    return;
                }

                policy.RequireAuthenticatedUser();

                policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalContentRead);
            });
        });
    }

    /// <summary>
    /// Gets a content item by an absolute route
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLName("contentByRoute")]
    [GraphQLDescription("Gets a content item by a route.")]
    public async Task<ContentItem?> ContentByRouteAsync(
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

        ContentItem? contentItem = await GetContentItemAsync(resolverContext, route, baseUrl).ConfigureAwait(false);

        if (contentItem != null)
        {
            return contentItem;
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContent? publishedContent = contentCache.GetByRoute(inContext.IncludePreview.Value, route, culture: inContext.Culture);
        contentItem = contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = StatusCodes.Status200OK,
        });

        return contentItem;
    }

    /// <summary>
    /// Resolves the content item
    /// </summary>
    /// <remarks>
    /// The default implementation uses the <see cref="IPublishedRouter"/> to resolve the content item
    /// and will create a redirect based on the values provided from this.
    /// </remarks>
    /// <exception cref="InvalidOperationException">If the route result isn't valid</exception>
    protected virtual async Task<ContentItem?> GetContentItemAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();
        IPublishedRouter publishedRouter = resolverContext.Service<IPublishedRouter>();
        IHttpContextAccessor? httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();

        baseUrl = SetBaseUrl(httpContextAccessor, baseUrl);

        IPublishedRequest contentRequest = await GetContentRequestAsync(publishedRouter, new Uri($"{baseUrl}{route}")).ConfigureAwait(false);

        ContentItem? contentItem;
        switch (contentRequest.GetRouteResult())
        {
            case UmbracoRouteResult.Success:
                if (contentRequest.PublishedContent == null)
                {
                    contentItem = null;
                    break;
                }

                contentItem = contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
                {
                    PublishedContent = contentRequest.PublishedContent,
                    ResolverContext = resolverContext,
                    Redirect = null,
                    StatusCode = StatusCodes.Status200OK,
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

    private static async Task<IPublishedRequest> GetContentRequestAsync(IPublishedRouter publishedRouter, Uri url)
    {
        IPublishedRequestBuilder builder = await publishedRouter.CreateRequestAsync(url).ConfigureAwait(false);
        IPublishedRequest request = await publishedRouter.RouteRequestAsync(builder, new RouteRequestOptions(RouteDirection.Inbound)).ConfigureAwait(false);

        return request;
    }
}
