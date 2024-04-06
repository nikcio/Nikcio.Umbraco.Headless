using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Members.Models;
using Nikcio.UHeadless.Members.Repositories;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.Members.Queries;

/// <summary>
/// Implements the <see cref="FindMembersByEmail"/> query
/// </summary>
/// <typeparam name="TMember"></typeparam>
public class FindMembersByEmailQuery<TMember>
    where TMember : IMember
{
    /// <summary>
    /// Finds members by email
    /// </summary>
    /// <param name="memberRepository"></param>
    /// <param name="email"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    [GraphQLDescription("Finds members by email.")]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMember?> FindMembersByEmail([Service] IMemberRepository<TMember> memberRepository,
                                            [GraphQLDescription("The email (may be partial).")] string email,
                                            [GraphQLDescription("The page index.")] long pageIndex,
                                            [GraphQLDescription("The page size.")] int pageSize,
                                            [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);

        return memberRepository.GetMemberList(x => x.FindByEmail(email, pageIndex, pageSize, out _, matchType));
    }
}
