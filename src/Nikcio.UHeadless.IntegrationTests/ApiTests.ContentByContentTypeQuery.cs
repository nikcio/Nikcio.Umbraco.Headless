using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _contentByContentTypeSnapshotPath = $"{SnapshotConstants.BasePath}/ContentByContentType";

    [Theory]
    [InlineData("test-1", "site", 1, 0, "en-US", false, null, true)]
    [InlineData("test-2", "default", 1, 0, "en-US", false, null, true)]
    [InlineData("test-3", "default", 1, 1, "en-US", false, null, true)]
    [InlineData("test-4", "default", 2, 1, "en-US", false, null, true)]
    [InlineData("test-5", "default", 1, 1000, "en-US", false, null, true)]
    [InlineData("test-6", "default", 1000, 1000, "en-US", false, null, true)]
    [InlineData("test-7", "default", 1, 5, "da", false, null, true)]
    [InlineData("test-8", "default", 1, 5, "en-US", true, null, true)]
    [InlineData("test-9", "default", 1, 5, "en-US", false, null, true)]
    [InlineData("test-10", "default", 1, 5, null, false, null, true)]
    [InlineData("test-11", "default", 1, 5, "da", null, null, true)]
    [InlineData("test-12", "default", 0, 5, "en-US", false, null, false)]
    [InlineData("test-13", "default", -1, 5, "en-US", false, null, false)]
    [InlineData("test-14", "default", 0, -1, "en-US", false, null, false)]
    [InlineData("test-15", "", 1, 1, "en-US", false, null, false)]
    public async Task ContentByContentTypeQuery_Snaps_Async(
        string testCase,
        string contentType,
        int page,
        int pageSize,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByContentTypeSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentByContentTypeQueries.GetItems,
            variables = new
            {
                contentType,
                page,
                pageSize,
                culture,
                includePreview,
                segment,
                baseUrl = "https://site-1.com"
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"ContentByContentType_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentByContentTypeQueries
{
    public const string GetItems = """
        query ContentByContentTypeQuery(
          $baseUrl: String!
          $page: Int!
          $pageSize: Int!
          $culture: String,
          $includePreview: Boolean
          $fallbacks: [PropertyFallback!]
          $segment: String,
          $contentType: String!
        ) {
          contentByContentType(contentType: $contentType, page: $page, pageSize: $pageSize, inContext: {
            culture: $culture,
            includePreview: $includePreview,
            fallbacks: $fallbacks,
            segment: $segment,
            baseUrl: $baseUrl
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
