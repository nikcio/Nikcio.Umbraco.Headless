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
    private const string _memberByEmailSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MemberByEmail";

    [Theory]
    [InlineData("user1@test.com", true, MemberByEmailQuery.ClaimValue)]
    [InlineData("user1@test.com", true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("user1@test.com", true, "Invalid")]
    public async Task MemberByEmailQuery_Snaps_Async(
        string email,
        bool expectSuccess,
        params string[] claims)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByEmailSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        JwtToken token = await CreateTokenMutation_Async(client, new TokenClaim() { Name = DefaultClaims.UHeadlessScope, Value = claims }).ConfigureAwait(true);

        client.DefaultRequestHeaders.Add(token.Header, token.Prefix + token.Token);

        using var request = JsonContent.Create(new
        {
            query = MemberByEmailQueries.GetItems,
            variables = new
            {
                email,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MemberByEmail_Snaps_{email}_{string.Join("-", claims)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
