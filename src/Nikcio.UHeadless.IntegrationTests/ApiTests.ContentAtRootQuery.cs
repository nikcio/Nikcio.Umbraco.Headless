using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _contentAtRootSnapshotPath = $"{SnapshotConstants.BasePath}/ContentAtRoot";

    [Theory]
    [InlineData("test-1", 1, 0, "en-us", false, null, true)]
    [InlineData("test-2", 1, 1, "en-us", false, null, true)]
    [InlineData("test-3", 2, 1, "en-us", false, null, true)]
    [InlineData("test-4", 1, 1000, "en-us", false, null, true)]
    [InlineData("test-5", 1000, 1000, "en-us", false, null, true)]
    [InlineData("test-6", 1, 5, "da", false, null, true)]
    [InlineData("test-7", 1, 5, "en-us", true, null, true)]
    [InlineData("test-8", 1, 5, "en-us", false, null, true)]
    [InlineData("test-9", 1, 5, null, false, null, true)]
    [InlineData("test-10", 1, 5, "da", null, null, true)]
    [InlineData("test-11", 0, 5, "en-us", false, null, false)]
    [InlineData("test-12", -1, 5, "en-us", false, null, false)]
    [InlineData("test-13", 0, -1, "en-us", false, null, false)]
    public async Task ContentAtRootQuery_Snaps_Async(
        string testCase,
        int page,
        int pageSize,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentAtRootQueries.GetItems,
            variables = new
            {
                page,
                pageSize,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentAtRootQueries
{
    public const string GetItems = """
        query ContentAtRootQuery(
          $page: Int!
          $pageSize: Int!
          $culture: String,
          $includePreview: Boolean
          $fallbacks: [PropertyFallback!]
          $segment: String
        ) {
          contentAtRoot(page: $page, pageSize: $pageSize, inContext: {
            culture: $culture,
            includePreview: $includePreview,
            fallbacks: $fallbacks,
            segment: $segment
          }) {
            items {
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
            page
            pageSize
            totalItems
            hasNextPage
          }
        }
        """ + Fragments.TypedProperties;
}
