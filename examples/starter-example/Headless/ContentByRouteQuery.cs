using HotChocolate.Resolvers;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace starter_example.Headless;
#pragma warning restore CA1707 // Identifiers should not contain underscores

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByRouteQuery : ContentByRouteQuery<ContentItemResult>
{
    protected override ContentItemResult? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<ContentItemResult> contentItemRepository, IResolverContext resolverContext)
    {
        return new ContentItemResult(new ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = StatusCodes.Status200OK,
        });
    }

    protected override async Task<ContentItemResult?> CreateContentItemFromRouteAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IPublishedRequest contentRequest = await GetContentRequestAsync(resolverContext, route, baseUrl).ConfigureAwait(false);

        IContentItemRepository<ContentItemResult> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItemResult>>();

        ContentItemResult? contentItem;
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
}

public class ContentItemResult : ContentItem
{
    public ContentItemResult(CreateCommand command) : base(command)
    {
    }

    public List<ContentItemResult?> Children(IResolverContext resolverContext)
    {
        return PublishedContent?.Children != null ? PublishedContent.Children.Select(child => CreateContentItem<ContentItemResult>(new CreateCommand()
        {
            PublishedContent = child,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = StatusCodes.Status200OK,
        }, DependencyReflectorFactory)).ToList() : [];
    }
}
