using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Authorization;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.ContentItems;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByTagQuery : ContentByTagQuery<ContentItem>
{
    protected override ContentItem? CreateContentItem(IPublishedContent publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            StatusCode = StatusCodes.Status200OK,
            Redirect = null
        });
    }
}

/// <summary>
/// Implements the <see cref="ContentByTag" /> query
/// </summary>
public abstract class ContentByTagQuery<TContentItem> : IGraphQLQuery
    where TContentItem : ContentItemBase
{
    public const string PolicyName = "ContentByTagQuery";

    public const string ClaimValue = "content.by.tag.query";

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

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalContentRead);
        });

        AvailableClaimValue availableClaimValue = new()
        {
            Name = DefaultClaims.UHeadlessScope,
            Values = [ClaimValue, DefaultClaimValues.GlobalContentRead]
        };
        AuthorizationTokenProvider.AddAvailableClaimValue(ClaimValueGroups.Content, availableClaimValue);
    }

    /// <summary>
    /// Gets content items by tag
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets content items by tag.")]
    public virtual PaginationResult<TContentItem?> ContentByTag(
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

        IContentItemRepository<TContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<TContentItem>>();
        ITagService tagService = resolverContext.Service<ITagService>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IEnumerable<TaggedEntity> taggedEntities = tagService.GetTaggedContentByTag(tag, tagGroup, inContext.Culture);
        IEnumerable<IPublishedContent> contentItems = taggedEntities.Select(entity => contentCache?.GetById(entity.EntityId)).OfType<IPublishedContent>();

        IEnumerable<TContentItem?> resultItems = contentItems.Select(contentItem => CreateContentItem(contentItem, contentItemRepository, resolverContext));

        return new PaginationResult<TContentItem?>(resultItems, page, pageSize);
    }

    protected abstract TContentItem? CreateContentItem(IPublishedContent publishedContent, IContentItemRepository<TContentItem> contentItemRepository, IResolverContext resolverContext);
}
