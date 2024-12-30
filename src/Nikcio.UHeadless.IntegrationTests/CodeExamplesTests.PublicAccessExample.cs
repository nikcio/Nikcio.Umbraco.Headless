using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class CodeExamplesTests
{
    private const string _publicAccessExampleSnapshotPath = $"{SnapshotConstants.CodeExamplesBasePath}/PublicAccessExample";

    [Theory]
    [InlineData("test-1", 1146, true)]
    [InlineData("test-2", 1152, true)]
    [InlineData("test-3", 1170, true)]
    [InlineData("test-4", 1148, true)]
    public async Task PublicAccessExampleQuery_Snaps_Async(
        string testCase,
        int id,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_publicAccessExampleSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = PublicAccessExampleQueries.GetItems,
            variables = new
            {
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"PublicAccessExample_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class PublicAccessExampleQueries
{
    public const string GetItems = """
        query PublicAccessExample($id: Int!) {
          publishAccessExampleQuery(id: $id) {
            permissions {
              urlLogin
              urlNoAccess
              accessRules {
                ruleType
                ruleValue
              }
            }
          }
        }
        """;
}
