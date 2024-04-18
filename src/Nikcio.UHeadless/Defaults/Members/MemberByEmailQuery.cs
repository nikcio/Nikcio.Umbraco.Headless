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
/// Implements the <see cref="MemberByEmail"/> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MemberByEmailQuery
{
    /// <summary>
    /// Gets a member by email
    /// </summary>
    [GraphQLDescription("Gets a member by email.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MemberItem? MemberByEmail(
        IResolverContext resolverContext,
        [Service] ILogger<MemberByEmailQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The email to fetch.")] string email)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);
        ArgumentNullException.ThrowIfNull(memberService);
        ArgumentException.ThrowIfNullOrEmpty(email);

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            logger.LogError("Member cache is null");
            return default;
        }

        IMember? member = memberService.GetByEmail(email);

        if (member == null)
        {
            logger.LogWarning("Member not found.");
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
