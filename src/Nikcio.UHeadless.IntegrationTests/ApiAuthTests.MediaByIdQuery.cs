using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.MediaItems;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _mediaByIdSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MediaById";

    [Theory]
    [InlineData("test-1", 1138, true, MediaByIdQuery.ClaimValue)]
    [InlineData("test-2", 1138, true, DefaultClaimValues.GlobalMediaRead)]
    [InlineData("test-3", 1138, true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task MediaByIdQuery_Snaps_Async(
        string testCase,
        int id,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByIdSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MediaByIdQueries.GetItems,
            variables = new
            {
                id,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MediaById_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
