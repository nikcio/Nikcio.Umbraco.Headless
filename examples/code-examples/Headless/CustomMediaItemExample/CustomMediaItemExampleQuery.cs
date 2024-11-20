using HotChocolate.Resolvers;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Code.Examples.Headless.CustomMediaItemExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class CustomMediaItemExampleQuery : MediaByIdQuery<MediaItem>
{
    [GraphQLName("CustomMediaItemExampleQuery")]
    public override MediaItem? MediaById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id)
    {
        return base.MediaById(resolverContext, id);
    }

    protected override MediaItem? CreateMediaItem(IPublishedContent? media, IMediaItemRepository<MediaItem> mediaItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        return mediaItemRepository.GetMediaItem(new Nikcio.UHeadless.Media.MediaItemBase.CreateCommand()
        {
            PublishedContent = media,
            ResolverContext = resolverContext,
        });
    }
}
