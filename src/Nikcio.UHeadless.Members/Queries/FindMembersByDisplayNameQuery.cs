using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="FindMembersByDisplayName"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class FindMembersByDisplayNameQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Finds members by display name
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="displayName"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    [GraphQLDescription("Finds members by display name.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMember?> FindMembersByDisplayName([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The display name (may be partial).")] string displayName,
                                            [GraphQLDescription("The page index.")] long pageIndex,
                                            [GraphQLDescription("The page size.")] int pageSize,
                                            [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        return memberRepository.GetMemberList(x => x.FindMembersByDisplayName(displayName, pageIndex, pageSize, out _, matchType));
    }
}
