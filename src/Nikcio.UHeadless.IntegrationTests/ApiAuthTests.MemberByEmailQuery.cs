using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _memberByEmailSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MemberByEmail";

    [Theory]
    [InlineData("test-1", "user1@test.com", true, MemberByEmailQuery.ClaimValue)]
    [InlineData("test-2", "user1@test.com", true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("test-3", "user1@test.com", true, "Invalid")]
    public async Task MemberByEmailQuery_Snaps_Async(
        string testCase,
        string email,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByEmailSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MemberByEmailQueries.GetItems,
            variables = new
            {
                email,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MemberByEmail_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
