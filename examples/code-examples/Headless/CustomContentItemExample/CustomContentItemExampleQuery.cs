using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Code.Examples.Headless.CustomContentItemExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class CustomContentItemExampleQuery : ContentByIdQuery<ContentItem>
{
    [GraphQLName("CustomContentItemExampleQuery")]
    public override ContentItem? ContentById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        return base.ContentById(resolverContext, id, inContext);
    }

    protected override ContentItem? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        return contentItemRepository.GetContentItem(new Nikcio.UHeadless.Defaults.ContentItems.ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            StatusCode = publishedContent == null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
            Redirect = null
        });
    }
}
