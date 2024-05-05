using System.Net.Http.Json;
using System.Text;

namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _contentByRouteSnapshotPath = $"{SnapshotConstants.BasePath}/ContentByRoute";

    [Theory]
    [InlineData("https://site-1.com", "/", null, false, null, true)]
    [InlineData("https://site-1.com", "/homepage", null, false, null, true)]
    [InlineData("https://site-1.com", "/page-1", null, false, null, true)]
    [InlineData("https://site-1.com", "/page-2", null, false, null, true)]
    [InlineData("https://site-1.com", "/collection-of-pages/block-list-page", null, false, null, true)]
    [InlineData("", "/no-domain-site", null, false, null, true)]
    [InlineData("", "/no-domain-homepage", null, false, null, true)]
    [InlineData("https://site-2.com", "/", null, false, null, true)]
    [InlineData("https://site-2.com", "/page-1", null, false, null, true)]
    [InlineData("https://site-culture.com", "/homepage", "en-us", false, null, true)]
    [InlineData("https://site-culture.dk", "/homepage", "da", false, null, true)]
    [InlineData("https://site-1.com", "/old-page", "en-us", false, null, true)]
    [InlineData("https://site-1.com", "/new-page", "en-us", false, null, true)]
    public async Task ContentByRouteQuery_Snaps_Async(
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

        string snapshotName = $"ContentByRoute_Snaps_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{culture}_{includePreview}_{segment}";

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
            baseUrl: $baseUrl,
            route: $route,
            inContext: {
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
