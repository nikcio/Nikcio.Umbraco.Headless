using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _contentByTagSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByTag";

    [Theory]
    //[InlineData("test-1", "normal", null, "en-us", false, null, true, ContentByTagQuery.ClaimValue)] // For some reason, this test fails only on the CI pipeline
    //[InlineData("test-2", "normal", null, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("test-3", "normal", null, "en-us", false, null, true, ContentByTagQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("test-4", "normal", null, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("test-5", "normal", null, "en-us", false, null, false, "Invalid")]
    public async Task ContentByTagQuery_Snaps_Async(
        string testCase,
        string tag,
        string? tagGroup,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByTagSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = ContentByTagQueries.GetItems,
            variables = new
            {
                tag,
                tagGroup,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByTag_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
