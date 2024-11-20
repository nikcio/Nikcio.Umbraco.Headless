using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _findMembersByUsernameSnapshotPath = $"{SnapshotConstants.AuthBasePath}/FindMembersByUsername";

    [Theory]
    [InlineData("test-1", "user1", StringPropertyMatchType.Exact, 1, 1, true, FindMembersByUsernameQuery.ClaimValue)]
    [InlineData("test-2", "user1", StringPropertyMatchType.Exact, 1, 1, true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("test-3", "user1", StringPropertyMatchType.Exact, 1, 1, false, "Invalid")]
    public async Task FindMembersByUsernameQuery_Snaps_Async(
        string testCase,
        string username,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByUsernameSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = FindMembersByUsernameQueries.GetItems,
            variables = new
            {
                username,
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"FindMembersByUsername_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
