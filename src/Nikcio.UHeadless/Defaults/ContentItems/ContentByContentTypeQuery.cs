using System.Diagnostics.CodeAnalysis;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Auth;
using Nikcio.UHeadless.Defaults.Authorization;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

/// <summary>
/// Implements the <see cref="ContentByContentType" /> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByContentTypeQuery : IGraphQLQuery
{
    public const string PolicyName = "ContentByContentTypeQuery";

    public const string ClaimValue = "content.by.contentType.query";

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
    /// Gets all the content items by content type
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets all the content items by content type.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<ContentItem?> ContentByContentType(
        IResolverContext resolverContext,
        [GraphQLDescription("The contentType to fetch.")] string contentType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(contentType);
        ArgumentNullException.ThrowIfNull(pageSize);
        ArgumentNullException.ThrowIfNull(page);

        inContext ??= new QueryContext();
        if (!inContext.Initialize(resolverContext))
        {
            throw new InvalidOperationException("The context could not be initialized");
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();
        IVariationContextAccessor variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContentType? publishedContentType = contentCache.GetContentType(contentType);
        if (publishedContentType == null)
        {
            ILogger<ContentByContentTypeQuery> logger = resolverContext.Service<ILogger<ContentByContentTypeQuery>>();
            logger.LogInformation("Content type not found");
            return new PaginationResult<ContentItem?>([], page, pageSize);
        }

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(inContext.IncludePreview.Value, inContext.Culture)
                    .SelectMany(content => content.DescendantsOrSelf(variationContextAccessor, inContext.Culture))
                    .Where(content => content.ContentType.Id == publishedContentType.Id);

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
