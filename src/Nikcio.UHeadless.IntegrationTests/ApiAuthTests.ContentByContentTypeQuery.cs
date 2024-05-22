using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
    private const string _contentByContentTypeSnapshotPath = $"{SnapshotConstants.AuthBasePath}/ContentByContentType";

    [Theory]
    [InlineData("default", 1, 1, "en-us", false, null, true, ContentByContentTypeQuery.ClaimValue)]
    [InlineData("default", 1, 1, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead)]
    [InlineData("default", 1, 1, "en-us", false, null, true, ContentByContentTypeQuery.ClaimValue, MemberPicker.ClaimValue)]
    [InlineData("default", 1, 1, "en-us", false, null, true, DefaultClaimValues.GlobalContentRead, MemberPicker.ClaimValue)]
    [InlineData("default", 1, 1, "en-us", false, null, false, "Invalid")]
    public async Task ContentByContentTypeQuery_Snaps_Async(
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
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByContentType_Snaps_{contentType}_{page}_{pageSize}_{culture}_{includePreview}_{segment}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
