using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
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
[ExtendObjectType(typeof(GraphQLQuery))]
public class FindMembersByRoleQuery
{
    /// <summary>
    /// Finds members by role
    /// </summary>
    [GraphQLDescription("Finds members by role.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public Pagination<MemberItem?> FindMembersByRole(
        IResolverContext resolverContext,
        [Service] ILogger<FindMembersByRoleQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The role name.")] string roleName,
        [GraphQLDescription("The username to match.")] string usernameToMatch,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);
        ArgumentNullException.ThrowIfNull(memberService);
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            logger.LogError("Member cache is null");
            return new Pagination<MemberItem?>(Enumerable.Empty<MemberItem>(), page, pageSize);
        }

        IEnumerable<IMember> members = memberService.FindMembersInRole(roleName, usernameToMatch, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        IEnumerable<MemberItem?> resultItems = memberItems.Select(member => memberItemRepository.GetMemberItem(new MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        }));

        return new Pagination<MemberItem?>(resultItems, page, pageSize);
    }
}
