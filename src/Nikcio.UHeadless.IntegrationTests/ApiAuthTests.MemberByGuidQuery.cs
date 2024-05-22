using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.IntegrationTests;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _memberByGuidSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MemberByGuid";

    [Theory]
    [InlineData("91eecdea-fc1e-408d-ac41-545ceefa3dc5", true, MemberByGuidQuery.ClaimValue)]
    [InlineData("91eecdea-fc1e-408d-ac41-545ceefa3dc5", true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("91eecdea-fc1e-408d-ac41-545ceefa3dc5", true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task MemberByGuidQuery_Snaps_Async(
        Guid key,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByGuidSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MemberByGuidQueries.GetItems,
            variables = new
            {
                key,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MemberByGuid_Snaps_{key}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
