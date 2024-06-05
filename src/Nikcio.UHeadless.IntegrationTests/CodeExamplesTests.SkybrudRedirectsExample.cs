using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class CodeExamplesTests
{
    private const string _skybrudRedirectsExampleSnapshotPath = $"{SnapshotConstants.CodeExamplesBasePath}/SkybrudRedirectsExample";

    [Theory]
    [InlineData("test-1", "https://site-2.com", "/skybrud/query-string", true)]
    [InlineData("test-2", "https://site-2.com", "/skybrud/query-string?q=my-query", true)]
    [InlineData("test-3", "https://site-2.com", "/skybrud", true)]
    [InlineData("test-4", "https://site-2.com", "/", true)]
    public async Task SkybrudRedirectsExampleQuery_Snaps_Async(
        string testCase,
        string baseUrl,
        string route,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_skybrudRedirectsExampleSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = SkybrudRedirectsExampleQueries.GetItems,
            variables = new
            {
                baseUrl,
                route
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"SkybrudRedirectsExample_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class SkybrudRedirectsExampleQueries
{
    public const string GetItems = """
        query SkybrudRedirectsExample($baseUrl: String!, $route: String!) {
          skybrudRedirectsExampleQuery(baseUrl: $baseUrl, route: $route) {
            name
            url(urlMode: ABSOLUTE)
            redirect {
              redirectUrl
              isPermanent
            }
            statusCode
          }
        }
        
        """;
}
