using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.IntegrationTests;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _mediaByGuidSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MediaByGuid";

    [Theory]
    [InlineData("25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true, MediaByGuidQuery.ClaimValue)]
    [InlineData("25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true, DefaultClaimValues.GlobalMediaRead)]
    [InlineData("25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task MediaByGuidQuery_Snaps_Async(
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

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByGuid_Snaps_{key}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
