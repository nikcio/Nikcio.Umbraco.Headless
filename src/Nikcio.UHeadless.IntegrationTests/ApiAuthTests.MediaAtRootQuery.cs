using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.MediaItems;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _mediaAtRootSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MediaAtRoot";

    [Theory]
    [InlineData("test-1", 1, 1, true, MediaAtRootQuery.ClaimValue)]
    [InlineData("test-2", 1, 1, true, DefaultClaimValues.GlobalMediaRead)]
    [InlineData("test-3", 1, 1, false, "Invalid")]
    public async Task MediaAtRootQuery_Snaps_Async(
        string testCase,
        int page,
        int pageSize,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaAtRootSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MediaAtRootQueries.GetItems,
            variables = new
            {
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MediaAtRoot_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
