using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;
using Umbraco.Cms.Core;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="MembersAll"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class MembersAllQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Gets all members by filter
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderDirection"></param>
    /// <param name="memberTypeAlias"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets all members by filter and/or pageindex.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMember?> MembersAll([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The current page index.")] long pageIndex,
                                            [GraphQLDescription("The page size.")] int pageSize,
                                            [GraphQLDescription("The field to order by.")] string orderBy,
                                            [GraphQLDescription("The direction to order by.")] Direction orderDirection,
                                            [GraphQLDescription("The member type alias to search for.")] string? memberTypeAlias = null,
                                            [GraphQLDescription("The search text filter.")] string? filter = null)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        filter ??= "";
        return memberRepository.GetMemberList(x => x.GetAll(pageIndex, pageSize, out _, orderBy, orderDirection, memberTypeAlias, filter));
    }
}
