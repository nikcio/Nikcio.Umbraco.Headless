using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Shared.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;

namespace Nikcio.UHeadless.Defaults.Content.Queries.ContentByRoute;

/// <summary>
/// Implements the <see cref="ContentByRouteAsync" /> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class ContentByRouteQuery
{
    /// <summary>
    /// Gets a content item by an absolute route
    /// </summary>
    /// <param name="contentItemRepository"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="publishedRouter"></param>
    /// <param name="resolverContext"></param>
    /// <param name="route"></param>
    /// <param name="baseUrl"></param>
    /// <param name="culture"></param>
    /// <param name="preview"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLName("ContentByRoute")]
    [GraphQLDescription("Gets a content item by a route.")]
    public async Task<ContentItem?> ContentByRouteAsync(
        IResolverContext resolverContext,
        [Service] IHttpContextAccessor httpContextAccessor,
        [Service] IPublishedRouter publishedRouter,
        [Service] IContentItemRepository<ContentItem> contentItemRepository,
        [GraphQLDescription("The route to fetch. Example '/da/frontpage/'.")] string route,
        [GraphQLDescription("The base url for the request. Example: 'https://localhost:4000'. Default is the current domain")] string baseUrl = "",
        [GraphQLDescription("The culture.")] string? culture = null,
        [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
        [GraphQLDescription("The property variation segment")] string? segment = null,
        [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(publishedRouter);
        ArgumentNullException.ThrowIfNull(contentItemRepository);
        ArgumentNullException.ThrowIfNull(resolverContext);

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
                    Culture = culture,
                    Segment = segment,
                    Fallback = fallback?.ToFallback(),
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
                    Culture = culture,
                    Segment = segment,
                    Fallback = fallback?.ToFallback(),
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
                    Culture = culture,
                    Segment = segment,
                    Fallback = fallback?.ToFallback(),
                    ResolverContext = resolverContext,
                    Redirect = null,
                    StatusCode = StatusCodes.Status404NotFound,
                });
                break;

            default:
                throw new InvalidOperationException("The route result is not valid");
        }

        if (contentItem != null)
        {
            return contentItem;
        }

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContent? publishedContent = contentCache.GetByRoute(preview, route, culture: culture);
        contentItem = contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            Culture = culture,
            Segment = segment,
            Fallback = fallback?.ToFallback(),
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = StatusCodes.Status200OK,
        });

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
