using System.Diagnostics.CodeAnalysis;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentByTag" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByTagQuery : IGraphQLQuery
{
    public const string PolicyName = "ContentByTagQuery";

    public const string ClaimValue = "content.by.tag.query";

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

                policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalContentRead);
            });
        });
    }

    /// <summary>
    /// Gets content items by tag
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets content items by tag.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<ContentItem?> ContentByTag(
        IResolverContext resolverContext,
        [GraphQLDescription("The tag to fetch.")] string tag,
        [GraphQLDescription("The tag group to fetch.")] string? tagGroup = null,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(tag);

        inContext ??= new QueryContext();
        if (!inContext.Initialize(resolverContext))
        {
            throw new InvalidOperationException("The context could not be initialized");
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();
        ITagService tagService = resolverContext.Service<ITagService>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IEnumerable<TaggedEntity> taggedEntities = tagService.GetTaggedContentByTag(tag, tagGroup, inContext.Culture);
        IEnumerable<IPublishedContent> contentItems = taggedEntities.Select(entity => contentCache?.GetById(entity.EntityId)).OfType<IPublishedContent>();

        IEnumerable<ContentItem?> resultItems = contentItems.Select(contentItem => contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = contentItem,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200
        }));

        return new PaginationResult<ContentItem?>(resultItems, page, pageSize);
    }
}
