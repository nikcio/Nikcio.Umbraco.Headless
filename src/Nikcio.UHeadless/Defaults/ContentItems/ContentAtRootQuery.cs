using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Authorization;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentAtRootQuery : ContentAtRootQuery<ContentItem>
{
    protected override ContentItem? CreateContentItem(IPublishedContent publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = StatusCodes.Status200OK
        });
    }
}

/// <summary>
/// Implements the <see cref="ContentAtRoot" /> query
/// </summary>
public abstract class ContentAtRootQuery<TContentItem> : IGraphQLQuery
    where TContentItem : ContentItemBase
{
    public const string PolicyName = "ContentAtRootQuery";

    public const string ClaimValue = "content.at.root.query";

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
    /// Gets all the content items at root level
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets all the content items at root level.")]
    public virtual PaginationResult<TContentItem?> ContentAtRoot(
        IResolverContext resolverContext,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        inContext ??= new QueryContext();
        if (!inContext.Initialize(resolverContext))
        {
            throw new InvalidOperationException("The context could not be initialized");
        }

        IContentItemRepository<TContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<TContentItem>>();
        IDocumentNavigationQueryService documentNavigationQueryService = resolverContext.Service<IDocumentNavigationQueryService>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        if (!documentNavigationQueryService.TryGetRootKeys(out IEnumerable<Guid>? rootKeys))
        {
            return new PaginationResult<TContentItem?>(
                [],
                page,
                pageSize
            );
        }

        List<IPublishedContent> contentItems = [];

        foreach (Guid key in rootKeys)
        {
            IPublishedContent? contentItem = contentCache.GetById(inContext.IncludePreview.Value, key);
            if (contentItem == null)
            {
                continue;
            }

            contentItems.Add(contentItem);
        }

        IEnumerable<TContentItem?> resultItems = contentItems.Select(contentItem => CreateContentItem(contentItem, contentItemRepository, resolverContext));

        return new PaginationResult<TContentItem?>(resultItems, page, pageSize);
    }

    protected abstract TContentItem? CreateContentItem(IPublishedContent publishedContent, IContentItemRepository<TContentItem> contentItemRepository, IResolverContext resolverContext);
}
