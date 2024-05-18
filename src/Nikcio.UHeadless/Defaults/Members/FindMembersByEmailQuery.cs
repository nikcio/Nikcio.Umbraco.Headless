using System.Diagnostics.CodeAnalysis;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.MemberItems;
using Nikcio.UHeadless.Members;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.Members;

/// <summary>
/// Implements the <see cref="FindMembersByEmail"/> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class FindMembersByEmailQuery : IGraphQLQuery
{
    public const string PolicyName = "FindMembersByEmailQuery";

    public const string ClaimValue = "find.members.by.email.query";

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

                policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalMemberRead);
            });
        });
    }

    /// <summary>
    /// Finds members by email
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Finds members by email.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<MemberItem?> FindMembersByEmail(
        IResolverContext resolverContext,
        [GraphQLDescription("The email (may be partial).")] string email,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentNullException.ThrowIfNull(pageSize);
        ArgumentNullException.ThrowIfNull(page);

        // We normalize the page to be 0-based because the repository is 0-based
        page--;

        if (page < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "The page must be greater than or equal to one");
        }

        if (pageSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size must be greater than zero");
        }

        IMemberItemRepository<MemberItem> memberItemRepository = resolverContext.Service<IMemberItemRepository<MemberItem>>();

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IMemberService memberService = resolverContext.Service<IMemberService>();

        IEnumerable<IMember> members = memberService.FindByEmail(email, page, pageSize, out long totalRecords, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        return memberItems.Select(member => memberItemRepository.GetMemberItem(new MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        }));
    }
}
