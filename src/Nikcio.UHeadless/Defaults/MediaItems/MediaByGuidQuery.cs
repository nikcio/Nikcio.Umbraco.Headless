using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.MediaItems;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MediaByGuidQuery : MediaByGuidQuery<MediaItem>
{
    protected override MediaItem? CreateMediaItem(IPublishedContent? media, IMediaItemRepository<MediaItem> mediaItemRepository, IResolverContext resolverContext)
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
/// Implements the <see cref="MediaByGuid" /> query
/// </summary>
public abstract class MediaByGuidQuery<TMediaItem> : IGraphQLQuery
    where TMediaItem : MediaItemBase
{
    public const string PolicyName = "MediaByGuidQuery";

    public const string ClaimValue = "media.by.guid.query";

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
    /// Gets a Media item by Guid
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets a Media item by Guid.")]
    public virtual TMediaItem? MediaByGuid(
        IResolverContext resolverContext,
        [GraphQLDescription("The Guid to fetch.")] Guid id)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IMediaItemRepository<TMediaItem> mediaItemRepository = resolverContext.Service<IMediaItemRepository<TMediaItem>>();

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContent? mediaItem = mediaCache.GetById(id);

        return CreateMediaItem(mediaItem, mediaItemRepository, resolverContext);
    }

    protected abstract TMediaItem? CreateMediaItem(IPublishedContent? media, IMediaItemRepository<TMediaItem> mediaItemRepository, IResolverContext resolverContext);
}
