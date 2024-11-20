using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _contentByRouteSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByRoute";

    [Theory]
    [InlineData("test-1", "https://site-1.com", "/homepage", null, false, null, true, ContentByRouteQuery.ClaimValue)]
    [InlineData("test-2", "https://site-1.com", "/homepage", null, false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("test-3", "https://site-1.com", "/homepage", null, false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("test-4", "https://site-1.com", "/homepage", null, false, null, true, ContentByRouteQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("test-5", "https://site-1.com", "/homepage", null, false, null, true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task ContentByRouteQuery_Snaps_Async(
        string testCase,
        string baseUrl,
        string route,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByRouteSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = ContentByRouteQueries.GetItems,
            variables = new
            {
                baseUrl,
                route,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByRoute_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
