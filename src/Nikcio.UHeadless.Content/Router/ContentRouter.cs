using Microsoft.AspNetCore.Http;
using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.Repositories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Content.Router;

/// <inheritdoc/>
public class ContentRouter<TContent, TContentRedirect> : IContentRouter<TContent, TContentRedirect>
    where TContent : IContent
    where TContentRedirect : IContentRedirect
{
    /// <summary>
    /// A content redirect repository
    /// </summary>
    protected IContentRedirectRepository<TContentRedirect> contentRedirectRepository { get; }

    /// <summary>
    /// A content repository
    /// </summary>
    protected IContentRepository<TContent> contentRepository { get; }

    /// <summary>
    /// The published router
    /// </summary>
    protected IPublishedRouter publishedRouter { get; }

    /// <summary>
    /// The httpcontext accessor
    /// </summary>
    protected IHttpContextAccessor httpContextAccessor { get; }

    /// <inheritdoc/>
    public ContentRouter(IContentRedirectRepository<TContentRedirect> contentRedirectRepository, IContentRepository<TContent> contentRepository, IPublishedRouter publishedRouter, IHttpContextAccessor httpContextAccessor)
    {
        this.contentRedirectRepository = contentRedirectRepository;
        this.contentRepository = contentRepository;
        this.publishedRouter = publishedRouter;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public virtual TContent? GetRedirect(IPublishedRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (request.RedirectUrl == null)
        {
            return default;
        }
        TContentRedirect? redirect = contentRedirectRepository.GetContentRedirect(new CreateContentRedirect(request.RedirectUrl, request.IsRedirectPermanent()));
        TContent? emptyContent = contentRepository.GetConvertedContent(null, null, null, null);

        if (emptyContent == null)
        {
            return default;
        }
        else
        {
            System.Reflection.PropertyInfo? redirectProperty = emptyContent.GetType().GetProperty(nameof(IContent.Redirect), typeof(TContentRedirect));
            if (redirectProperty == null)
            {
                return default;
            }
            redirectProperty.SetValue(emptyContent, redirect);
            return emptyContent;
        }
    }

    /// <inheritdoc/>
    public virtual async Task<TContent?> GetContentByRouting(string route, string baseUrl, string? culture, string? segment, Fallback? fallback)
    {
        IPublishedRequestBuilder builder = await publishedRouter.CreateRequestAsync(new Uri($"{baseUrl}{route}")).ConfigureAwait(false);
        IPublishedRequest request = await publishedRouter.RouteRequestAsync(builder, new RouteRequestOptions(RouteDirection.Inbound)).ConfigureAwait(false);

        return request.GetRouteResult() switch
        {
            UmbracoRouteResult.Redirect => GetRedirect(request),
            UmbracoRouteResult.NotFound => default,
            UmbracoRouteResult.Success => request.PublishedContent != null ? contentRepository.GetConvertedContent(request.PublishedContent, culture, segment, fallback) : default,
            _ => default,
        };
    }

    /// <inheritdoc/>
    public virtual string SetBaseUrl(string baseUrl)
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

    /// <inheritdoc/>
    public virtual TContent? GetContentByRouteCache(string route, string? culture, bool preview, string? segment, Fallback? fallback)
    {
        return contentRepository.GetContent(x => x?.GetByRoute(preview, route, culture: culture), culture, segment, fallback);
    }

    /// <inheritdoc/>
    public IEnumerable<TContent?> GetContentDescendantsByRouteCache(string route, string? culture, bool preview, string? segment, Fallback? fallback)
    {
        return contentRepository.GetContentList(x => x?.GetByRoute(preview, route, culture: culture)?.Descendants(culture) ?? Enumerable.Empty<IPublishedContent>(), culture, segment, fallback);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TContent?>> GetContentDescendantsByRouting(string route, string baseUrl, string? culture, string? segment, Fallback? fallback)
    {
        IPublishedRequestBuilder builder = await publishedRouter.CreateRequestAsync(new Uri($"{baseUrl}{route}")).ConfigureAwait(false);
        IPublishedRequest request = await publishedRouter.RouteRequestAsync(builder, new RouteRequestOptions(RouteDirection.Inbound)).ConfigureAwait(false);

        return request.GetRouteResult() switch
        {
            UmbracoRouteResult.Redirect => GetRedirect(request).AsEnumerableOfOne() ?? Enumerable.Empty<TContent>(),
            UmbracoRouteResult.NotFound => Enumerable.Empty<TContent>(),
            UmbracoRouteResult.Success => request.PublishedContent != null ? request.PublishedContent.Descendants(culture).Select(content => contentRepository.GetConvertedContent(content, culture, segment, fallback)) : Enumerable.Empty<TContent>(),
            _ => Enumerable.Empty<TContent>(),
        };
    }
}
