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
/// Implements the <see cref="MemberByGuid"/> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MemberByGuidQuery
{
    /// <summary>
    /// Gets a member by Guid
    /// </summary>
    [GraphQLDescription("Gets a member by Guid.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MemberItem? MemberByGuid(
        IResolverContext resolverContext,
        [Service] ILogger<MemberByGuidQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The id to fetch.")] Guid id)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);
        ArgumentNullException.ThrowIfNull(memberService);
        ArgumentNullException.ThrowIfNull(id);

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            logger.LogError("Member cache is null");
            return default;
        }

        IMember? member = memberService.GetByKey(id);

        if (member == null)
        {
            logger.LogWarning("Member not found. {Id}", id);
            return default;
        }

        IPublishedContent? memberItem = memberCache.Get(member);

        return memberItemRepository.GetMemberItem(new MemberBase.CreateCommand()
        {
            PublishedContent = memberItem,
            ResolverContext = resolverContext,
        });
    }
}
