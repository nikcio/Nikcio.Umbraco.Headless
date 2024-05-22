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
    private const string _contentByIdSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentById";

    [Theory]
    [InlineData(1146, "en-us", false, null, true, ContentByIdQuery.ClaimValue)]
    [InlineData(1146, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData(1152, "en-us", false, null, true, ContentByIdQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData(1152, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData(1152, "en-us", false, null, true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task ContentByIdQuery_Snaps_Async(
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
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentById_Snaps_{id}_{culture}_{includePreview}_{segment}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
