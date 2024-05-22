using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _mediaAtRootSnapshotPath = $"{SnapshotConstants.BasePath}/MediaAtRoot";

    [Theory]
    [InlineData(1, 0, true)]
    [InlineData(1, 1, true)]
    [InlineData(2, 1, true)]
    [InlineData(1, 1000, true)]
    [InlineData(1000, 1000, true)]
    [InlineData(1, 5, true)]
    [InlineData(0, 5, false)]
    [InlineData(-1, 5, false)]
    [InlineData(0, -1,  false)]
    public async Task MediaAtRootQuery_Snaps_Async(
        int page,
        int pageSize,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaAtRootSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MediaAtRootQueries.GetItems,
            variables = new
            {
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaAtRoot_Snaps_{page}_{pageSize}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MediaAtRootQueries
{
    public const string GetItems = """
        query MediaAtRootQuery(
          $page: Int!,
          $pageSize: Int!
        ) {
          mediaAtRoot(
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
