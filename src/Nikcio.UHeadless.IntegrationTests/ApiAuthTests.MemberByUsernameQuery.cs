using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _memberByUsernameSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MemberByUsername";

    [Theory]
    [InlineData("test-1", "user1", true, MemberByUsernameQuery.ClaimValue)]
    [InlineData("test-2", "user1", true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("test-3", "user1", true, "Invalid")]
    public async Task MemberByUsernameQuery_Snaps_Async(
        string testCase,
        string username,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByUsernameSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MemberByUsernameQueries.GetItems,
            variables = new
            {
                username,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MemberByUsername_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
