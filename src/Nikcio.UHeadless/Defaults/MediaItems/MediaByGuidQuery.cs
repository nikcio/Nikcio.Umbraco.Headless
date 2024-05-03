using System.Diagnostics.CodeAnalysis;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Media;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Defaults.MediaItems;

/// <summary>
/// Implements the <see cref="MediaByGuid" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MediaByGuidQuery : IGraphQLQuery
{
    public const string PolicyName = "MediaByGuidQuery";

    public const string ClaimValue = "media.by.guid.query";

    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorization(configure =>
        {
            configure.AddPolicy(PolicyName, policy =>
            {
                if (options.DisableAuthorization)
                {
                    policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
                    return;
                }

                policy.RequireAuthenticatedUser();

                policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalMediaRead);
            });
        });
    }

    /// <summary>
    /// Gets a Media item by Guid
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets a Media item by Guid.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MediaItem? MediaByGuid(
        IResolverContext resolverContext,
        [GraphQLDescription("The Guid to fetch.")] Guid id)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IMediaItemRepository<MediaItem> mediaItemRepository = resolverContext.Service<IMediaItemRepository<MediaItem>>();

        IPublishedMediaCache? mediaCache = mediaItemRepository.GetCache();

        if (mediaCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContent? mediaItem = mediaCache.GetById(id);

        return mediaItemRepository.GetMediaItem(new MediaItemBase.CreateCommand()
        {
            PublishedContent = mediaItem,
            ResolverContext = resolverContext,
        });
    }
}
