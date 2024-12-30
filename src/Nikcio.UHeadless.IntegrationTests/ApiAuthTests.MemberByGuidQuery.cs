using System.Net.Http.Json;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiAuthTests
{
    private const string _memberByGuidSnapshotPath = $"{SnapshotConstants.AuthBasePath}/MemberByGuid";

    [Theory]
    [InlineData("test-1", "91eecdea-fc1e-408d-ac41-545ceefa3dc5", true, MemberByGuidQuery.ClaimValue)]
    [InlineData("test-2", "91eecdea-fc1e-408d-ac41-545ceefa3dc5", true, DefaultClaimValues.GlobalMemberRead)]
    [InlineData("test-3", "91eecdea-fc1e-408d-ac41-545ceefa3dc5", true, "Invalid")] // Doesn't error because null is a vaild response
    public async Task MemberByGuidQuery_Snaps_Async(
        string testCase,
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

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MemberByGuid_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}
