using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Auth;

namespace Nikcio.UHeadless.Defaults.Authorization;

/// <summary>
/// Implements the <see cref="UtilityClaimGroupsQuery"/> query
/// </summary>
/// <remarks>
/// Requires .AddAuth() to be called on <see cref="UHeadlessOptions"/>.
/// </remarks>
[ExtendObjectType(typeof(HotChocolateQueryObject))]
public sealed class UtilityClaimGroupsQuery : IGraphQLQuery
{
    public void ApplyConfiguration(UHeadlessOptions options)
    {
        // No configuration needed
    }

    [GraphQLName("utility_GetClaimGroups")]
    [GraphQLDescription("Utility query. Gets a which claims are used by the registered queries.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public IEnumerable<ClaimGroup> UtilityGetClaimGroups(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IAuthorizationTokenProvider authorizationTokenProvider = resolverContext.Service<IAuthorizationTokenProvider>();

        return authorizationTokenProvider.GetAvailableClaims().Select(claimGroup => new ClaimGroup()
        {
            GroupName = claimGroup.GroupName,
            ClaimValues = claimGroup.ClaimValues.Select(claim => new ClaimValue()
            {
                Name = claim.Name,
                Values = claim.Values
            })
        });
    }
}

[GraphQLDescription("A group of claims.")]
public sealed class ClaimGroup
{
    [GraphQLDescription("The name of the group.")]
    public required string GroupName { get; init; }

    [GraphQLDescription("The claim values in the group.")]
    public required IEnumerable<ClaimValue> ClaimValues { get; init; }
}

public sealed class ClaimValue
{
    [GraphQLDescription("The name of the claim.")]
    public required string Name { get; init; }

    [GraphQLDescription("The available values of the claim.")]
    public required List<string> Values { get; init; }
}
