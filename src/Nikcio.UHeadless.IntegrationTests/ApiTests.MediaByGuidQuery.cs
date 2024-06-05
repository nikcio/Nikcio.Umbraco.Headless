using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _mediaByGuidSnapshotPath = $"{SnapshotConstants.BasePath}/MediaByGuid";

    [Theory]
    [InlineData("test-1", "25ba1577-a0c5-4329-8f32-9e9abe4a6d2d", true)]
    [InlineData("test-2", "01164aab-e1f7-4628-9e20-870b49951482", true)]
    [InlineData("test-3", "b6113530-0ccf-4006-88d4-3dcd31c8297e", true)]
    [InlineData("test-4", "d6355171-2e04-4b73-b11b-01f408ec242a", true)]
    [InlineData("test-5", "abc", false)]
    public async Task MediaByGuidQuery_Snaps_Async(
        string testCase,
        string key,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByGuidSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MediaByGuidQueries.GetItems,
            variables = new
            {
                key,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByGuid_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MediaByGuidQueries
{
    public const string GetItems = """
        query MediaByGuidQuery($key: UUID!) {
          mediaByGuid(id: $key) {
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
        }
        """ + Fragments.CustomMediaType;
}
