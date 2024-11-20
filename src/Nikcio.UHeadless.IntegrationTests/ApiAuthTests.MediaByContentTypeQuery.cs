using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.MediaItems;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _mediaByContentTypeSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MediaByContentType";

    [Theory]
    [InlineData("test-1", "image", 1, 1, true, MediaByContentTypeQuery.ClaimValue)]
    [InlineData("test-2", "image", 1, 1, true, DefaultClaimValues.GlobalMediaRead)]
    [InlineData("test-3", "image", 1, 1, false, "Invalid")]
    public async Task MediaByContentTypeQuery_Snaps_Async(
        string testCase,
        string contentType,
        int page,
        int pageSize,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MediaByContentTypeQueries.GetItems,
            variables = new
            {
                contentType,
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
