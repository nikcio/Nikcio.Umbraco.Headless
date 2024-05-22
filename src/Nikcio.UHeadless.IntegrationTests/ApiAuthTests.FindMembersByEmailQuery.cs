using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.IntegrationTests.Defaults;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _findMembersByEmailSnapshotPath = $"{SnapshotConstants.AuthBasePath}/FindMembersByEmail";

    [Theory]
    [InlineData("user1@test.com", StringPropertyMatchType.Exact, 1, 1, true, FindMembersByEmailQuery.ClaimValue)]
    [InlineData("user1@test.com", StringPropertyMatchType.Exact, 1, 1, true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("user1@test.com", StringPropertyMatchType.Exact, 1, 1, false, "Invalid")]
    public async Task FindMembersByEmailQuery_Snaps_Async(
        string email,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByEmailSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = FindMembersByEmailQueries.GetItems,
            variables = new
            {
                email,
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"FindMembersByEmail_Snaps_{email}_{matchType}_{page}_{pageSize}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
