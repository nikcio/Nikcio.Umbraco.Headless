using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;
public partial class ApiAuthTests
{
    private const string _utilityGetClaimGroupsSnapshotPath = $"{SnapshotConstants.AuthBasePath}/UtilityGetClaimGroups";

    [Fact]
    public async Task Utility_GetClaimGroups_Snaps_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_utilityGetClaimGroupsSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = UtilityGetClaimGroupsQueries.GetItems,
            variables = new
            {
            }
        });
        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);
        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"Utility_GetClaimGroups_Snaps.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}

public static class UtilityGetClaimGroupsQueries
{
    public const string GetItems = """
        query {
          utility_GetClaimGroups {
            groupName
            claimValues {
              name
              values
            }
          }
        }
        """;
}
