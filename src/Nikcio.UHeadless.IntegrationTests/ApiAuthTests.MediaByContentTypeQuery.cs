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
    private const string _mediaByContentTypeSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MediaByContentType";

    [Theory]
    [InlineData("image", 1, 1, true, MediaByContentTypeQuery.ClaimValue)]
    [InlineData("image", 1, 1, true, DefaultClaimValues.GlobalMediaRead)]
    [InlineData("image", 1, 1, false, "Invalid")]
    public async Task MediaByContentTypeQuery_Snaps_Async(
        string contentType,
        int page,
        int pageSize,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MediaByContentTypeQueries.GetItems,
            variables = new
            {
                contentType,
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_Snaps_{contentType}_{page}_{pageSize}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
