using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Nikcio.UHeadless.Defaults.Authorization;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    [Theory]
    [InlineData("name", "value", null)]
    [InlineData("scope", """{"A": "B", "C": 1, "D": true}""", TokenClaimType.Json)]
    [InlineData("scope", """["A", "B", "C"]""", TokenClaimType.JsonArray)]
    [InlineData("scope", """[1, 2, 3]""", TokenClaimType.JsonArray)]
    public async Task CreateTokenMutation_WithSingleClaim_Async(string name, string value, TokenClaimType? type)
    {
        HttpClient client = _factory.CreateClient();

        var claims = new TokenClaim[]
        {
            new()
            {
                Name = name,
                Value = type == null ? value : JsonSerializer.Deserialize<object>(value) ?? throw new InvalidOperationException("Cannot deserialize JSON."),
                Type = type
            }
        };

        JwtToken token = await CreateTokenMutation_Async(client, claims).ConfigureAwait(true);

        Assert.NotNull(token);
        Assert.NotEmpty(token.Token);
        Assert.True(token.Expires > 0);
        Assert.Equal(DefaultHeaders.Token, token.Header);
        Assert.Equal("Bearer ", token.Prefix);
    }

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<JwtToken> CreateTokenMutation_Async(HttpClient client, params TokenClaim[] claims)
    {
        ArgumentNullException.ThrowIfNull(client);

        using var request = JsonContent.Create(new
        {
            query = CreateTokenMutations.CreateToken,
            variables = new
            {
                claims = claims.Select(claim => new
                {
                    claim.Name,
                    claim.Value,
                    Type = claim.Type.HasValue ? TestUtils.GetHotChocolateEnum(claim.Type.Value.ToString()) : null
                }).ToArray()
            }
        });

        request.Headers.Add(DefaultHeaders.ApiKey, AuthenticatedApplicationFactory.ApiKey);

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        Assert.NotEmpty(responseContent);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        JwtTokenResponse jwtTokenResponse = await response.Content.ReadFromJsonAsync<JwtTokenResponse>(_jsonSerializerOptions).ConfigureAwait(true) ?? throw new InvalidOperationException("Failed to read token from response.");
        return jwtTokenResponse.Data.CreateToken;
    }

    private sealed class JwtTokenResponse
    {
        public required DataObject Data { get; init; }

        public sealed class DataObject
        {
            public required JwtToken CreateToken { get; init; }
        }
    }
}

public static class CreateTokenMutations
{
    public const string CreateToken = """
        mutation CreateTokenMutation($claims: [TokenClaimInput!]!) {
          createToken(claims: $claims) {
            expires
            token
            header
            prefix
          }
        }
        """;
}

public static partial class TestUtils
{
    public static string GetHotChocolateEnum(string enumAsString)
    {
        string result = CapitalLettersRegex().Replace(enumAsString, "_$1").ToUpperInvariant();

        if (result.StartsWith('_'))
        {
            result = result[1..];
        }

        return result;
    }

    [GeneratedRegex(@"([A-Z])")]
    private static partial Regex CapitalLettersRegex();
}
