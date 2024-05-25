using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using UrlTracker.Core;
using UrlTracker.Web.Processing;

namespace Code.Examples.Headless.UrlTrackerExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class UrlTrackerExampleQuery : ContentByRouteQuery
{
    protected override ContentItem? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        IRequestInterceptFilterCollection requestFilters = resolverContext.Service<IRequestInterceptFilterCollection>();
        IResponseInterceptHandlerCollection interceptHandlers = resolverContext.Service<IResponseInterceptHandlerCollection>();

        IInterceptService interceptService = resolverContext.Service<IInterceptService>();
        UrlTracker.Core.Intercepting.Models.IIntercept a = interceptService.GetAsync(UrlTracker.Core.Domain.Models.Url.Parse("https://site.com/home")).Result;

        return base.CreateContentItem(publishedContent, contentItemRepository, resolverContext);
    }

    protected override Task<ContentItem?> CreateContentItemFromRouteAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        return base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl);
    }
}
