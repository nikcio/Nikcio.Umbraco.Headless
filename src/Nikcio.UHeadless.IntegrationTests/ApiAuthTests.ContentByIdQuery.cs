using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _contentByIdSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentById";

    [Theory]
    [InlineData("test-1", 1146, "en-US", false, null, true, ContentByIdQuery.ClaimValue)]
    [InlineData("test-2", 1146, "en-US", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("test-3", 1152, "en-US", false, null, true, ContentByIdQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("test-4", 1152, "en-US", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("test-5", 1152, "en-US", false, null, true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task ContentByIdQuery_Snaps_Async(
        string testCase,
        int id,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByIdSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = ContentByIdQueries.GetItems,
            variables = new
            {
                id,
                culture,
                includePreview,
                segment,
                baseUrl = "https://site-1.com"
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"ContentById_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
