using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Defaults.Auth;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

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
        ArgumentNullException.ThrowIfNull(pageSize);
        ArgumentNullException.ThrowIfNull(page);

        IMediaItemRepository<TMediaItem> mediaItemRepository = resolverContext.Service<IMediaItemRepository<TMediaItem>>();

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContentType? mediaContentType = mediaCache.GetContentType(contentType);

        if (mediaContentType == null)
        {
            ILogger<MediaByContentTypeQuery> logger = resolverContext.Service<ILogger<MediaByContentTypeQuery>>();
            logger.LogError("Media type not found. {ContentType}", contentType);
            return new PaginationResult<TMediaItem?>([], page, pageSize);
        }

        IEnumerable<IPublishedContent> mediaItems = mediaCache.GetByContentType(mediaContentType);

        IEnumerable<TMediaItem?> resultItems = mediaItems.Select(mediaItem => CreateMediaItem(mediaItem, mediaItemRepository, resolverContext));

        return new PaginationResult<TMediaItem?>(resultItems, page, pageSize);
    }

    protected abstract TMediaItem? CreateMediaItem(IPublishedContent media, IMediaItemRepository<TMediaItem> mediaItemRepository, IResolverContext resolverContext);
}
