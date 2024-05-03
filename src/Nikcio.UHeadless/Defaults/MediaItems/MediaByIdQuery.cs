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
/// Implements the <see cref="MediaById" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MediaByIdQuery : IGraphQLQuery
{
    public const string PolicyName = "MediaByIdQuery";

    public const string ClaimValue = "media.by.id.query";

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
    /// Gets a Media item by id
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets a Media item by id.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MediaItem? MediaById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The id must be greater than zero");
        }

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
