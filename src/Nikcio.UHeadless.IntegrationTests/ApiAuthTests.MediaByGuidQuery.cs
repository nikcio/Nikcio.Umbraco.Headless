using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.MediaItems;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _mediaByGuidSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MediaByGuid";

    [Theory]
    [InlineData("test-1", "25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true, MediaByGuidQuery.ClaimValue)]
    [InlineData("test-2", "25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true, DefaultClaimValues.GlobalMediaRead)]
    [InlineData("test-3", "25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task MediaByGuidQuery_Snaps_Async(
        string testCase,
        string key,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByGuidSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MediaByGuidQueries.GetItems,
            variables = new
            {
                key,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MediaByGuid_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
