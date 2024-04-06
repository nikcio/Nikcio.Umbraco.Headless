using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="MemberByUsername"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class MemberByUsernameQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Gets a member by username
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets a member by username.")]
    [UseFiltering]
    [UseSorting]
    public virtual TMember? MemberByUsername([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The username to fetch.")] string username)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        return memberRepository.GetMember(x => x.GetByUsername(username));
    }
}
