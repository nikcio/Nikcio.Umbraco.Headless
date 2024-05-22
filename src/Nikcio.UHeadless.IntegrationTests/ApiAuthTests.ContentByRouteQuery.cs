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
    private const string _contentByRouteSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByRoute";

    [Theory]
    [InlineData("https://site-1.com", "/homepage", null, false, null, true, ContentByRouteQuery.ClaimValue)]
    [InlineData("https://site-1.com", "/homepage", null, false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("https://site-1.com", "/homepage", null, false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("https://site-1.com", "/homepage", null, false, null, true, ContentByRouteQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("https://site-1.com", "/homepage", null, false, null, true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task ContentByRouteQuery_Snaps_Async(
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

        string snapshotName = $"ContentByRoute_Snaps_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{culture}_{includePreview}_{segment}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
