using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="FindMembersByUsername"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class FindMembersByUsernameQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Finds members by username
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="username"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    [GraphQLDescription("Finds members by username.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMember?> FindMembersByUsername([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The username (may be partial).")] string username,
                                            [GraphQLDescription("The page index.")] long pageIndex,
                                            [GraphQLDescription("The page size.")] int pageSize,
                                            [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType)
    {
        ArgumentNullException.ThrowIfNull(memberRepository, nameof(memberRepository));

        return memberRepository.GetMemberList(x => x.FindByUsername(username, pageIndex, pageSize, out _, matchType));
    }
}
