using System.Text.RegularExpressions;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlTracker.Core;
using UrlTracker.Core.Domain.Models;
using UrlTracker.Core.Intercepting.Models;
using UrlTracker.Core.Models;
using UrlTracker.Web.Processing;

namespace Code.Examples.Headless.UrlTrackerExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class UrlTrackerExampleQuery : ContentByRouteQuery
{
    [GraphQLName("urlTrackerExampleQuery")]
    public override Task<ContentItem?> ContentByRouteAsync(
        IResolverContext resolverContext,
        [GraphQLDescription("The route to fetch. Example '/da/frontpage/'.")] string route,
        [GraphQLDescription("The base url for the request. Example: 'https://localhost:4000'. Default is the current domain")] string baseUrl = "",
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        return base.ContentByRouteAsync(resolverContext, route, baseUrl, inContext);
    }

    protected override async Task<ContentItem?> CreateContentItemFromRouteAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentNullException.ThrowIfNull(baseUrl);

        IRequestInterceptFilterCollection requestInterceptFilters = resolverContext.Service<IRequestInterceptFilterCollection>();

        var url = Url.Parse($"{baseUrl.TrimEnd('/')}{route}");

        if (!await requestInterceptFilters.EvaluateCandidateAsync(url).ConfigureAwait(false))
        {
            return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
        }

        IInterceptService interceptService = resolverContext.Service<IInterceptService>();

        IIntercept intercept = await interceptService.GetAsync(url).ConfigureAwait(false);

        if (intercept.Info is not Redirect redirect)
        {
            return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        IPublishedUrlProvider publishedUrlProvider = resolverContext.Service<IPublishedUrlProvider>();
        string? culture = resolverContext.Culture();

        int statusCode = (int) redirect.TargetStatusCode;
        string? redirectUrl = redirect.TargetUrl != null ? redirect.TargetUrl : redirect.TargetNode?.Url(publishedUrlProvider, culture, UrlMode.Absolute);

        if (redirectUrl == null)
        {
            statusCode = StatusCodes.Status410Gone;
        }
        else if (redirect.RetainQuery)
        {
            redirectUrl = redirectUrl.TrimEnd('/') + url.Query;
        }

        if (redirectUrl != null && string.IsNullOrWhiteSpace(redirect.SourceUrl) && !string.IsNullOrWhiteSpace(redirect.SourceRegex))
        {
            redirectUrl = Regex.Replace((url.Path + url.Query).TrimStart('/'), redirect.SourceRegex, redirectUrl);
        }

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = null,
            ResolverContext = resolverContext,
            Redirect = new()
            {
                IsPermanent = statusCode == StatusCodes.Status301MovedPermanently,
                RedirectUrl = redirectUrl,
            },
            StatusCode = statusCode,
        });
    }
}
