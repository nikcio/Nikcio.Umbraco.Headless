using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Members;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.Members;

/// <summary>
/// Implements the <see cref="FindMembersByDisplayName"/> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class FindMembersByDisplayNameQuery
{
    /// <summary>
    /// Finds members by display name
    /// </summary>
    [GraphQLDescription("Finds members by display name.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<MemberItem?> FindMembersByDisplayName(
        IResolverContext resolverContext,
        [Service] ILogger<FindMembersByDisplayNameQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The display name (may be partial).")] string displayName,
        [GraphQLDescription("The page index.")] long pageIndex,
        [GraphQLDescription("The page size.")] int pageSize,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);
        ArgumentNullException.ThrowIfNull(memberService);
        ArgumentException.ThrowIfNullOrEmpty(displayName);

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            logger.LogError("Member cache is null");
            return Enumerable.Empty<MemberItem>();
        }

        IEnumerable<IMember> members = memberService.FindMembersByDisplayName(displayName, pageIndex, pageSize, out long totalRecords, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        return memberItems.Select(member => memberItemRepository.GetMemberItem(new MemberBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        }));
    }
}
