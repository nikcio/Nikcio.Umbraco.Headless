using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentAtRoot" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentAtRootQuery : IGraphQLQuery
{
    public const string PolicyName = "ContentAtRootQuery";

    public const string ClaimValue = "content.at.root.query";

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
    /// Gets all the content items at root level
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets all the content items at root level.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<ContentItem?> ContentAtRoot(
        IResolverContext resolverContext,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentNullException.ThrowIfNull(pageSize);
        ArgumentNullException.ThrowIfNull(page);

        inContext ??= new QueryContext();
        if (!inContext.Initialize(resolverContext))
        {
            throw new InvalidOperationException("The context could not be initialized");
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(inContext.IncludePreview.Value, inContext.Culture);

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
