using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.Properties;
using Nikcio.UHeadless.IntegrationTests;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _contentByTagSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByTag";

    [Theory]
    //[InlineData("normal", null, "en-us", false, null, true, ContentByTagQuery.ClaimValue)] // For some reason, this test fails only on the CI pipeline
    //[InlineData("normal", null, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("normal", null, "en-us", false, null, true, ContentByTagQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("normal", null, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("normal", null, "en-us", false, null, false, "Invalid")]
    public async Task ContentByTagQuery_Snaps_Async(
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

        string snapshotName = $"ContentByTag_Snaps_{tag}_{tagGroup}_{culture}_{includePreview}_{segment}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
