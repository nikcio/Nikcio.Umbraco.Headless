using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _contentByContentTypeSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByContentType";

    [Theory]
    [InlineData("test-1", "default", 1, 1, "en-us", false, null, true, ContentByContentTypeQuery.ClaimValue)]
    [InlineData("test-2", "default", 1, 1, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("test-3", "default", 1, 1, "en-us", false, null, true, ContentByContentTypeQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("test-4", "default", 1, 1, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("test-5", "default", 1, 1, "en-us", false, null, false, "Invalid")]
    public async Task ContentByContentTypeQuery_Snaps_Async(
        string testCase,
        string contentType,
        int page,
        int pageSize,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByContentTypeSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = ContentByContentTypeQueries.GetItems,
            variables = new
            {
                contentType,
                page,
                pageSize,
                culture,
                includePreview,
                segment,
                baseUrl = "https://site-1.com"
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"ContentByContentType_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
