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
/// Implements the <see cref="FindMembersByRole"/> query
/// </summary>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class FindMembersByRoleQuery : IGraphQLQuery
{
    public const string PolicyName = "FindMembersByRoleQuery";

    public const string ClaimValue = "find.members.by.role.query";

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
    /// Finds members by role
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Finds members by role.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public PaginationResult<MemberItem?> FindMembersByRole(
        IResolverContext resolverContext,
        [GraphQLDescription("The role name.")] string roleName,
        [GraphQLDescription("The username to match.")] string usernameToMatch,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(roleName);
        ArgumentNullException.ThrowIfNull(pageSize);
        ArgumentNullException.ThrowIfNull(page);

        IMemberItemRepository<MemberItem> memberItemRepository = resolverContext.Service<IMemberItemRepository<MemberItem>>();

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IMemberService memberService = resolverContext.Service<IMemberService>();

        IEnumerable<IMember> members = memberService.FindMembersInRole(roleName, usernameToMatch, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        IEnumerable<MemberItem?> resultItems = memberItems.Select(member => memberItemRepository.GetMemberItem(new MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        }));

        return new PaginationResult<MemberItem?>(resultItems, page, pageSize);
    }
}
