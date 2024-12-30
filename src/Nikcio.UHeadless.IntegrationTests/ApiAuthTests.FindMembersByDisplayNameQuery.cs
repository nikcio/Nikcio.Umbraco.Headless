using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _findMembersByDisplayNameSnapshotPath = $"{SnapshotConstants.AuthBasePath}/FindMembersByDisplayName";

    [Theory]
    [InlineData("test-1", "user1", StringPropertyMatchType.Exact, 1, 1, true, FindMembersByDisplayNameQuery.ClaimValue)]
    [InlineData("test-2", "user1", StringPropertyMatchType.Exact, 1, 1, true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("test-3", "user1", StringPropertyMatchType.Exact, 1, 1, false, "Invalid")]
    public async Task FindMembersByDisplayNameQuery_Snaps_Async(
        string testCase,
        string displayName,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByDisplayNameSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = FindMembersByDisplayNameQueries.GetItems,
            variables = new
            {
                displayName,
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"FindMembersByDisplayName_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
