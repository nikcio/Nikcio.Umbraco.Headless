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
/// Implements the <see cref="MemberById"/> query
/// </summary>
[ExtendObjectType(typeof(GraphQLQuery))]
public class MemberByIdQuery
{
    /// <summary>
    /// Gets a member by id
    /// </summary>
    [GraphQLDescription("Gets a member by id.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public MemberItem? MemberById(
        IResolverContext resolverContext,
        [Service] ILogger<MemberByIdQuery> logger,
        [Service] IMemberRepository<MemberItem> memberItemRepository,
        [Service] IMemberService memberService,
        [GraphQLDescription("The id to fetch.")] int id)
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

        IMember? member = memberService.GetById(id);

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
