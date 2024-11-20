using HotChocolate.Resolvers;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Services;

namespace Code.Examples.Headless.SkybrudRedirectsExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class SkybrudRedirectsExampleQuery : ContentByRouteQuery
{
    [GraphQLName("skybrudRedirectsExampleQuery")]
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

        IRedirectsService redirectService = resolverContext.Service<IRedirectsService>();

        var uri = new Uri($"{baseUrl.TrimEnd('/')}{route}");
        IRedirect? redirect = redirectService.GetRedirectByUri(uri);

        if (redirect != null)
        {
            IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

            string redirectUrl = redirect.Destination.FullUrl;

            if (redirect.ForwardQueryString)
            {
                redirectUrl = redirectUrl.TrimEnd('/') + uri.Query;
            }

            return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
            {
                PublishedContent = null,
                ResolverContext = resolverContext,
                Redirect = new()
                {
                    IsPermanent = redirect.IsPermanent,
                    RedirectUrl = redirectUrl,
                },
                StatusCode = redirect.IsPermanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status307TemporaryRedirect,
            });
        }

        return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
    }
}
