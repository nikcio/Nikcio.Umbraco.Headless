using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _contentByRouteSnapshotPath = $"{SnapshotConstants.BasePath}/ContentByRoute";

    [Theory]
    [InlineData("test-1", "https://site-1.com", "/", null, false, null, true)]
    [InlineData("test-2", "https://site-1.com", "/homepage", null, false, null, true)]
    [InlineData("test-3", "https://site-1.com", "/page-1", null, false, null, true)]
    [InlineData("test-4", "https://site-1.com", "/page-2", null, false, null, true)]
    [InlineData("test-5", "https://site-1.com", "/collection-of-pages/block-list-page", null, false, null, true)]
    [InlineData("test-6", "", "/no-domain-site", null, false, null, true)]
    [InlineData("test-7", "", "/no-domain-homepage", null, false, null, true)]
    [InlineData("test-8", "https://site-2.com", "/", null, false, null, true)]
    [InlineData("test-9", "https://site-2.com", "/page-1", null, false, null, true)]
    [InlineData("test-10", "https://site-culture.com", "/homepage", "en-us", false, null, true)]
    [InlineData("test-11", "https://site-culture.dk", "/homepage", "da", false, null, true)]
    [InlineData("test-12", "https://site-1.com", "/old-page", "en-us", false, null, true)]
    [InlineData("test-13", "https://site-1.com", "/new-page", "en-us", false, null, true)]
    public async Task ContentByRouteQuery_Snaps_Async(
        string testCase,
        string baseUrl,
        string route,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByRouteSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentByRouteQueries.GetItems,
            variables = new
            {
                baseUrl,
                route,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByRoute_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentByRouteQueries
{
    public const string GetItems = """
        query ContentByRouteQuery(
          $baseUrl: String!,
          $route: String!,
          $culture: String
          $includePreview: Boolean
          $fallbacks: [PropertyFallback!]
          $segment: String
        ) {
          contentByRoute(
            route: $route,
            inContext: {
              baseUrl: $baseUrl,
              culture: $culture
              includePreview: $includePreview
              fallbacks: $fallbacks
              segment: $segment
            }
          ) {
            url(urlMode: ABSOLUTE)
            redirect {
              redirectUrl
              isPermanent
            }
            statusCode
            properties {
              ...typedProperties
              __typename
            }
            urlSegment
            name
            id
            key
            templateId
            parent {
              url(urlMode: ABSOLUTE)
              properties {
                ...typedProperties
                __typename
              }
              urlSegment
              name
              id
              key
              templateId
            }
            __typename
          }
        }
        """ + Fragments.TypedProperties;
}
