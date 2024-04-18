using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.MemberItems;
using Nikcio.UHeadless.Members;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.Members;

/// <summary>
/// Implements the <see cref="MemberByUsername"/> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MemberByUsernameQuery
{
    /// <summary>
    /// Gets a member by username
    /// </summary>
    [GraphQLDescription("Gets a member by username.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MemberItem? MemberByUsername(
        IResolverContext resolverContext,
        [Service] ILogger<MemberByUsernameQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The username to fetch.")] string username)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);
        ArgumentNullException.ThrowIfNull(memberService);
        ArgumentException.ThrowIfNullOrEmpty(username);

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            logger.LogError("Member cache is null");
            return default;
        }

        IMember? member = memberService.GetByUsername(username);

        if (member == null)
        {
            logger.LogWarning("Member not found. {Username}", username);
            return default;
        }

        IPublishedContent? memberItem = memberCache.Get(member);

        return memberItemRepository.GetMemberItem(new MemberItemBase.CreateCommand()
        {
            PublishedContent = memberItem,
            ResolverContext = resolverContext,
        });
    }
}
