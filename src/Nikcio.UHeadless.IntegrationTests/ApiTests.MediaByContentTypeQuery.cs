using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _mediaByContentTypeSnapshotPath = $"{SnapshotConstants.BasePath}/MediaByContentType";

    [Theory]
    [InlineData("test-1", "image", 1, 0, true)]
    [InlineData("test-2", "folder", 1, 0, true)]
    [InlineData("test-3", "customMediaType", 1, 10, true)]
    [InlineData("test-4", "image", 1, 1, true)]
    [InlineData("test-5", "image", 2, 1, true)]
    [InlineData("test-6", "image", 1, 1000, true)]
    [InlineData("test-7", "image", 1000, 1000, true)]
    [InlineData("test-8", "image", 1, 5, true)]
    [InlineData("test-9", "image", 0, 5, false)]
    [InlineData("test-10", "image", -1, 5, false)]
    [InlineData("test-11", "image", 0, -1, false)]
    [InlineData("test-12", "", 1, 1, false)]
    public async Task MediaByContentTypeQuery_Snaps_Async(
        string testCase,
        string contentType,
        int page,
        int pageSize,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MediaByContentTypeQueries.GetItems,
            variables = new
            {
                contentType,
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MediaByContentTypeQueries
{
    public const string GetItems = """
        query MediaByContentTypeQuery(
          $contentType: String!
          $page: Int!,
          $pageSize: Int!
        ) {
          mediaByContentType(
            contentType: $contentType
            page: $page,
            pageSize: $pageSize
          ) {
            items {
              url(urlMode: ABSOLUTE)
              properties {
                ...customMediaType
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
                  ...customMediaType
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
        """ + Fragments.CustomMediaType;
}
