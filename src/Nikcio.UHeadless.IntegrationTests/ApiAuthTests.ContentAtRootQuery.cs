using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.IntegrationTests.Defaults;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _contentAtRootSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentAtRoot";

    [Theory]
    [InlineData(1, 1, "en-us", false, null, true, ContentAtRootQuery.ClaimValue)]
    [InlineData(1, 1, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData(1, 1, "en-us", false, null, false, "Invalid")]
    public async Task ContentAtRootQuery_Snaps_Async(
        int page,
        int pageSize,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = ContentAtRootQueries.GetItems,
            variables = new
            {
                page,
                pageSize,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_Snaps_{page}_{pageSize}_{culture}_{includePreview}_{segment}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
