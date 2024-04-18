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
/// Implements the <see cref="FindMembersByEmail"/> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class FindMembersByEmailQuery
{
    /// <summary>
    /// Finds members by email
    /// </summary>
    [GraphQLDescription("Finds members by email.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<MemberItem?> FindMembersByEmail(
        IResolverContext resolverContext,
        [Service] ILogger<FindMembersByEmailQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The email (may be partial).")] string email,
        [GraphQLDescription("The page index.")] long pageIndex,
        [GraphQLDescription("The page size.")] int pageSize,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);
        ArgumentNullException.ThrowIfNull(memberService);
        ArgumentException.ThrowIfNullOrEmpty(email);

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            logger.LogError("Member cache is null");
            return Enumerable.Empty<MemberItem>();
        }

        IEnumerable<IMember> members = memberService.FindByEmail(email, pageIndex, pageSize, out long totalRecords, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        return memberItems.Select(member => memberItemRepository.GetMemberItem(new MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        }));
    }
}
