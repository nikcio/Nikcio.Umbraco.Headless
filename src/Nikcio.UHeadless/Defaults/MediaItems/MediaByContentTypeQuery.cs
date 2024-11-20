using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.MediaItems;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MediaByContentTypeQuery : MediaByContentTypeQuery<MediaItem>
{
    protected override MediaItem? CreateMediaItem(IPublishedContent media, IMediaItemRepository<MediaItem> mediaItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        return mediaItemRepository.GetMediaItem(new MediaItemBase.CreateCommand()
        {
            PublishedContent = media,
            ResolverContext = resolverContext,
        });
    }
}

/// <summary>
/// Implements the <see cref="MediaByContentType" /> query
/// </summary>
public abstract class MediaByContentTypeQuery<TMediaItem> : IGraphQLQuery
    where TMediaItem : MediaItemBase
{
    public const string PolicyName = "MediaByContentTypeQuery";

    public const string ClaimValue = "media.by.contentType.query";

    [GraphQLIgnore]
    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorizationBuilder().AddPolicy(PolicyName, policy =>
        {
            if (options.DisableAuthorization)
            {
                policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
                return;
            }

            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadless);

            policy.RequireAuthenticatedUser();

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalMediaRead);
        });

        AvailableClaimValue availableClaimValue = new()
        {
            Name = DefaultClaims.UHeadlessScope,
            Values = [ClaimValue, DefaultClaimValues.GlobalMediaRead]
        };
        AuthorizationTokenProvider.AddAvailableClaimValue(ClaimValueGroups.Media, availableClaimValue);
    }

    /// <summary>
    /// Gets all the media items by content type
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets all the media items by content type.")]
    public virtual PaginationResult<TMediaItem?> MediaByContentType(
        IResolverContext resolverContext,
        [GraphQLDescription("The content type to fetch.")] string contentType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        IMediaItemRepository<TMediaItem> mediaItemRepository = resolverContext.Service<IMediaItemRepository<TMediaItem>>();
        IPublishedContentTypeCache publishedContentTypeCache = resolverContext.Service<IPublishedContentTypeCache>();
        IDocumentNavigationQueryService documentNavigationQueryService = resolverContext.Service<IDocumentNavigationQueryService>();

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }


        IPublishedContentType? publishedContentType = publishedContentTypeCache.Get(PublishedItemType.Media, contentType);
        if (publishedContentType == null)
        {
            ILogger<MediaByContentTypeQuery> logger = resolverContext.Service<ILogger<MediaByContentTypeQuery>>();
            logger.LogInformation("Media type not found. {ContentType}", contentType);
            return new PaginationResult<TMediaItem?>([], page, pageSize);
        }

        if (!documentNavigationQueryService.TryGetRootKeys(out IEnumerable<Guid>? rootKeys))
        {
            return new PaginationResult<TMediaItem?>(
                [],
                page,
                pageSize
            );
        }
        List<IPublishedContent> mediaItems = [];

        foreach (Guid key in rootKeys)
        {
            IPublishedContent? mediaItem = mediaCache.GetById(key);
            if (mediaItem == null)
            {
                continue;
            }

            mediaItems.Add(mediaItem);
        }

        IEnumerable<IPublishedContent> mediaItemsOfContentType = mediaItems
            .SelectMany(content => content.DescendantsOrSelf())
            .Where(content => content.ContentType.Id == publishedContentType.Id);

        IEnumerable<TMediaItem?> resultItems = mediaItems.Select(mediaItem => CreateMediaItem(mediaItem, mediaItemRepository, resolverContext));

        return new PaginationResult<TMediaItem?>(resultItems, page, pageSize);
    }

    protected abstract TMediaItem? CreateMediaItem(IPublishedContent media, IMediaItemRepository<TMediaItem> mediaItemRepository, IResolverContext resolverContext);
}
