using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="MembersById"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class MembersByIdQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Gets a members by id
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets members by id.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMember?> MembersById([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The ids to fetch.")] int[] ids)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        return memberRepository.GetMemberList(x => x.GetAllMembers(ids));
    }
}
