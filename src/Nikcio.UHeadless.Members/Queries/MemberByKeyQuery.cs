using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="MemberByKey"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class MemberByKeyQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Gets a member by key
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets a member by key.")]
    [UseFiltering]
    [UseSorting]
    public virtual TMember? MemberByKey([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The key to fetch.")] Guid key)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        return memberRepository.GetMember(x => x.GetByKey(key));
    }
}
