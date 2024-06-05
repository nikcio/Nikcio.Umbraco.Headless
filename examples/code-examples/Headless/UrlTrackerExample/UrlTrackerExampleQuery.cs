using System.Text.RegularExpressions;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.Options;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlTracker.Core;
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

        var requestedUrl = Url.Parse($"{baseUrl.TrimEnd('/')}{route}");

        if (!await requestInterceptFilters.EvaluateCandidateAsync(requestedUrl).ConfigureAwait(false))
        {
            return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
        }

        IInterceptService interceptService = resolverContext.Service<IInterceptService>();

        IIntercept intercept = await interceptService.GetAsync(requestedUrl).ConfigureAwait(false);

        if (intercept.Info is not Redirect redirect)
        {
            return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        string? redirectUrl = redirect.Target switch
        {

            UrlTargetStrategy target => GetUrl(resolverContext, redirect, target, requestedUrl),
            ContentPageTargetStrategy target => GetUrl(resolverContext, redirect, target, requestedUrl),
            _ => throw new NotImplementedException(),
        };

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = null,
            ResolverContext = resolverContext,
            Redirect = new()
            {
                IsPermanent = redirect.Permanent,
                RedirectUrl = redirectUrl,
            },
            StatusCode = redirectUrl == null ? StatusCodes.Status410Gone : GetStatusCode(redirect),
        });
    }

    private static int GetStatusCode(Redirect redirect)
    {
        return redirect.Permanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status307TemporaryRedirect;
    }

    private static string? GetUrl(IResolverContext resolverContext, Redirect redirect, UrlTargetStrategy target, Url requestedUrl)
    {
        string urlString = target.Url;

        if (redirect.Source is RegexSourceStrategy regexsource)
        {
            urlString = Regex.Replace((requestedUrl.Path + requestedUrl.Query).TrimStart('/'), regexsource.Value, urlString, RegexOptions.None, TimeSpan.FromMilliseconds(100));
        }

        var url = Url.Parse(urlString);

        if (redirect.RetainQuery)
        {
            url.Query = requestedUrl.Query;
        }

        if (url.AvailableUrlTypes.Contains(UrlTracker.Core.Models.UrlType.Absolute))
        {
            RequestHandlerSettings requestHandlerSettingsValue = resolverContext.Service<IOptions<RequestHandlerSettings>>().Value;
            return url.ToString(UrlTracker.Core.Models.UrlType.Absolute, requestHandlerSettingsValue.AddTrailingSlash);
        }
        else
        {
            return url.ToString();
        }
    }

    private static string? GetUrl(IResolverContext resolverContext, Redirect redirect, ContentPageTargetStrategy target, Url requestedUrl)
    {
        if (target.Content is null)
        {
            return null;
        }

        IPublishedUrlProvider publishedUrlProvider = resolverContext.Service<IPublishedUrlProvider>();
        var url = Url.Parse(target.Content.Url(publishedUrlProvider, target.Culture.DefaultIfNullOrWhiteSpace(null), UrlMode.Absolute));

        if (redirect.RetainQuery)
        {
            url.Query = requestedUrl.Query;
        }

        if (url.AvailableUrlTypes.Contains(UrlTracker.Core.Models.UrlType.Absolute))
        {
            RequestHandlerSettings requestHandlerSettingsValue = resolverContext.Service<IOptions<RequestHandlerSettings>>().Value;
            return url.ToString(UrlTracker.Core.Models.UrlType.Absolute, requestHandlerSettingsValue.AddTrailingSlash);
        }
        else
        {
            return url.ToString();
        }
    }
}
