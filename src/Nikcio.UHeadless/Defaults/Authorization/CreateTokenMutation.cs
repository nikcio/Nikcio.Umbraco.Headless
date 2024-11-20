using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Nikcio.UHeadless.Defaults.Authorization;

[ExtendObjectType(typeof(HotChocolateMutationObject))]
internal class CreateTokenMutation : IGraphQLMutation
{
    public const string PolicyName = "CreateTokenMutation";

    public void ApplyConfiguration(UHeadlessOptions options)
    {
        options.UmbracoBuilder.Services.AddAuthorizationBuilder().AddPolicy(PolicyName, policy =>
        {
            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadlessApiKey);

            policy.RequireAuthenticatedUser();
        });
    }

    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Creates a JWT token to be used for other queries.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static will remove this query from GraphQL")]
    public JwtToken CreateToken(
        IResolverContext resolverContext,
        [GraphQLDescription("The claims of the token.")] TokenClaim[] claims)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IAuthorizationTokenProvider authorizationTokenProvider = resolverContext.Service<IAuthorizationTokenProvider>();

        System.Security.Claims.Claim[] claimsArray = claims.Select(claim =>
        {
            string? valueType = claim.Type switch
            {
                TokenClaimType.Json => JsonClaimValueTypes.Json,
                TokenClaimType.JsonArray => JsonClaimValueTypes.JsonArray,
                _ => JsonClaimValueTypes.Json
            };

            return new System.Security.Claims.Claim(claim.Name, JsonSerializer.Serialize(claim.Value), valueType);
        }).ToArray();

        JwtSecurityToken token = authorizationTokenProvider.CreateToken(claimsArray);

        return new JwtToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = new DateTimeOffset(token.ValidTo).ToUnixTimeSeconds()
        };
    }
}

[GraphQLDescription("A claim for a token.")]
public sealed class TokenClaim
{
    [GraphQLDescription("The name of the claim.")]
    public required string Name { get; init; }

    [GraphQLType(typeof(AnyType))]
    [GraphQLDescription("The value of the claim.")]
    public required object Value { get; init; }

    [GraphQLDescription("The type of claim.")]
    public TokenClaimType? Type { get; init; }
}

[GraphQLDescription("The type of claim.")]
public enum TokenClaimType
{
    Json,
    JsonArray
}

[GraphQLDescription("A JWT token.")]
public sealed class JwtToken
{
    [GraphQLDescription("The JWT token.")]
    public required string Token { get; init; }

    [GraphQLDescription("The expiration time of the token in Unix timestamp.")]
    public required long Expires { get; init; }

    [GraphQLDescription("The prefix used when using the token.")]
    public string Prefix { get; } = "Bearer ";

    [GraphQLDescription("The header used when using the token.")]
    public string Header { get; } = DefaultHeaders.Token;
}
