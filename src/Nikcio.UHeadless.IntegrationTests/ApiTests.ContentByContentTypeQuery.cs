using System.Net.Http.Json;
using Nikcio.UHeadless.Properties;

namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _contentByContentTypeSnapshotPath = $"{SnapshotConstants.BasePath}/ContentByContentType";

    [Theory]
    [InlineData("site", 1, 0, "en-us", false, null, true)]
    [InlineData("default", 1, 0, "en-us", false, null, true)]
    [InlineData("default", 1, 1, "en-us", false, null, true)]
    [InlineData("default", 2, 1, "en-us", false, null, true)]
    [InlineData("default", 1, 1000, "en-us", false, null, true)]
    [InlineData("default", 1000, 1000, "en-us", false, null, true)]
    [InlineData("default", 1, 5, "da", false, null, true)]
    [InlineData("default", 1, 5, "en-us", true, null, true)]
    [InlineData("default", 1, 5, "en-us", false, null, true)]
    [InlineData("default", 1, 5, null, false, null, true)]
    [InlineData("default", 1, 5, "da", null, null, true)]
    [InlineData("default", 0, 5, "en-us", false, null, false)]
    [InlineData("default", -1, 5, "en-us", false, null, false)]
    [InlineData("default", 0, -1, "en-us", false, null, false)]
    [InlineData("", 1, 1, "en-us", false, null, false)]
    public async Task ContentByContentTypeQuery_Snaps_Async(
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
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByContentType_Snaps_{contentType}_{page}_{pageSize}_{culture}_{includePreview}_{segment}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentByContentTypeQueries
{
    public const string GetItems = """
        query ContentByContentTypeQuery(
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
