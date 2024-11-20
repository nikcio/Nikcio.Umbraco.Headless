//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;

//namespace Nikcio.UHeadless.IntegrationTests;

//public partial class CodeExamplesTests
//{
//    private const string _urlTrackerExampleSnapshotPath = $"{SnapshotConstants.CodeExamplesBasePath}/UrlTrackerExample";

//    [Theory]
//    [InlineData("test-1", "https://site-1.com", "/url-tracker/basic-redirect", true)]
//    [InlineData("test-2", "https://site-1.com", "/url-tracker/regexipsum", true)]
//    [InlineData("test-3", "https://site-1.com", "/url-tracker/query", true)]
//    [InlineData("test-4", "https://site-1.com", "/url-tracker/query?q=my-query", true)]
//    [InlineData("test-5", "https://site-1.com", "/url-tracker/redirect-url", true)]
//    [InlineData("test-6", "https://site-1.com", "/url-tracker", true)]
//    [InlineData("test-7", "https://site-1.com", "/", true)]
//    public async Task UrlTrackerExampleQuery_Snaps_Async(
//        string testCase,
//        string baseUrl,
//        string route,
//        bool expectSuccess)
//    {
//        var snapshotProvider = new SnapshotProvider($"{_urlTrackerExampleSnapshotPath}/Snaps");
//        HttpClient client = _factory.CreateClient();

//        using var request = JsonContent.Create(new
//        {
//            query = UrlTrackerExampleQueries.GetItems,
//            variables = new
//            {
//                baseUrl,
//                route
//            }
//        });

//        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

//        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

//        string snapshotName = $"UrlTrackerExample_Snaps_{testCase}.snap";

//        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
//        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
//    }

//    [Theory]
//    [InlineData("test-1", "https://example-site.com/not-found", "2024-05-30T14:10:20+01:00", null, 404, true)]
//    [InlineData("test-2", "https://example-site.com/not-found", "2024-05-30T14:10:20+01:00", null, 500, true)]
//    [InlineData("test-3", "https://example-site.com/not-found", "2024-05-30T14:10:20+01:00", "https://site-example.com/not-found", 404, true)]
//    public async Task UrlTrackerExampleQuery_TrackErrorStatusCode_Async(
//        string testCase,
//        string url,
//        string timeStamp,
//        string? referrer,
//        int statusCode,
//        bool expectSuccess)
//    {
//        var snapshotProvider = new SnapshotProvider($"{_urlTrackerExampleSnapshotPath}/TrackErrorStatusCode");
//        HttpClient client = _factory.CreateClient();

//        using var request = JsonContent.Create(new
//        {
//            query = UrlTrackerExampleQueries.TrackErrorStatusCode,
//            variables = new
//            {
//                url,
//                timeStamp,
//                referrer,
//                statusCode
//            }
//        });

//        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

//        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

//        string snapshotName = $"UrlTrackerExample_TrackErrorStatusCode_{testCase}.snap";

//        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
//        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
//    }
//}

//public static class UrlTrackerExampleQueries
//{
//    public const string GetItems = """
//        query UrlTrackerExample($baseUrl: String!, $route: String!) {
//          urlTrackerExampleQuery(baseUrl: $baseUrl, route: $route) {
//            redirect {
//              redirectUrl
//              isPermanent
//            }
//            name
//            statusCode
//          }
//        }
//        """;

//    public const string TrackErrorStatusCode = """
//        mutation TrackErrorStatusCode(
//          $url: String!
//          $timeStamp: DateTime!
//          $referrer: String
//          $statusCode: Int!
//        ) {
//          trackErrorStatusCode(
//            url: $url
//            timestamp: $timeStamp
//            referrer: $referrer
//            statusCode: $statusCode
//          ) {
//            success
//          }
//        }
//        """;
//}
