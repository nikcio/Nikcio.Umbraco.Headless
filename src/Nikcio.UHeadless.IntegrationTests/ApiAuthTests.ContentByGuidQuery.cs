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
    private const string _contentByGuidSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByGuid";

    [Theory]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", "en-us", false, null, true, ContentByGuidQuery.ClaimValue)]
    [InlineData("ca69a30f-bf47-4acf-b31b-556c585d204b", "en-us", false, null, true, ContentByGuidQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("ca69a30f-bf47-4acf-b31b-556c585d204b", "en-us", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", "en-us", false, null, true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task ContentByGuidQuery_Snaps_Async(
        string key,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByGuidSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = ContentByGuidQueries.GetItems,
            variables = new
            {
                key,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByGuid_Snaps_{key}_{culture}_{includePreview}_{segment}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
