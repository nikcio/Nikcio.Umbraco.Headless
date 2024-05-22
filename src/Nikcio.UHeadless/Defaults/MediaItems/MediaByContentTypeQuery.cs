using System.Diagnostics.CodeAnalysis;
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

/// <summary>
/// Implements the <see cref="MediaByContentType" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MediaByContentTypeQuery : IGraphQLQuery
{
    public const string PolicyName = "MediaByContentTypeQuery";

    public const string ClaimValue = "media.by.contentType.query";

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
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<MediaItem?> MediaByContentType(
        IResolverContext resolverContext,
        [GraphQLDescription("The content type to fetch.")] string contentType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(contentType);
        ArgumentNullException.ThrowIfNull(pageSize);
        ArgumentNullException.ThrowIfNull(page);

        IMediaItemRepository<MediaItem> mediaItemRepository = resolverContext.Service<IMediaItemRepository<MediaItem>>();

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
            return new PaginationResult<MediaItem?>([], page, pageSize);
        }

        IEnumerable<IPublishedContent> mediaItems = mediaCache.GetByContentType(mediaContentType);

        IEnumerable<MediaItem?> resultItems = mediaItems.Select(mediaItem => mediaItemRepository.GetMediaItem(new MediaItemBase.CreateCommand()
        {
            PublishedContent = mediaItem,
            ResolverContext = resolverContext,
        }));

        return new PaginationResult<MediaItem?>(resultItems, page, pageSize);
    }
}
