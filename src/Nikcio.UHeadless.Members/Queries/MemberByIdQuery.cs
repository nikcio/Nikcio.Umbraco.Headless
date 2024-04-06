using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="MemberById"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class MemberByIdQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Gets a member by id
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets a member by id.")]
    [UseFiltering]
    [UseSorting]
    public virtual TMember? MemberById([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The id to fetch.")] int id)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        return memberRepository.GetMember(x => x.GetById(id));
    }
}
