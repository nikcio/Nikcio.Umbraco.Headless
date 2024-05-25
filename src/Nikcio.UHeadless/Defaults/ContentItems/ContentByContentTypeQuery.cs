using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Auth;
using Nikcio.UHeadless.Defaults.Authorization;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class ContentByContentTypeQuery : ContentByContentTypeQuery<ContentItem>
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
/// Implements the <see cref="ContentByContentType" /> query
/// </summary>
public abstract class ContentByContentTypeQuery<TContentItem> : IGraphQLQuery
    where TContentItem : ContentItemBase
{
    public const string PolicyName = "ContentByContentTypeQuery";

    public const string ClaimValue = "content.by.contentType.query";

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
    /// Gets all the content items by content type
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets all the content items by content type.")]
    public virtual PaginationResult<TContentItem?> ContentByContentType(
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

        IContentItemRepository<TContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<TContentItem>>();
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
            return new PaginationResult<TContentItem?>([], page, pageSize);
        }

        IEnumerable<IPublishedContent> contentItems = contentCache.GetAtRoot(inContext.IncludePreview.Value, inContext.Culture)
                    .SelectMany(content => content.DescendantsOrSelf(variationContextAccessor, inContext.Culture))
                    .Where(content => content.ContentType.Id == publishedContentType.Id);

        IEnumerable<TContentItem?> resultItems = contentItems.Select(contentItem => CreateContentItem(contentItem, contentItemRepository, resolverContext));

        return new PaginationResult<TContentItem?>(resultItems, page, pageSize);
    }

    protected abstract TContentItem? CreateContentItem(IPublishedContent publishedContent, IContentItemRepository<TContentItem> contentItemRepository, IResolverContext resolverContext);
}
