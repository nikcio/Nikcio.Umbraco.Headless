using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _mediaByIdSnapshotPath = $"{SnapshotConstants.BasePath}/MediaById";

    [Theory]
    [InlineData(1138, true)]
    [InlineData(1141, true)]
    [InlineData(1144, true)]
    [InlineData(1143, true)]
    [InlineData(-1000, true)]
    [InlineData(100_000_000, true)]
    public async Task MediaByIdQuery_Snaps_Async(
        int id,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByIdSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MediaByIdQueries.GetItems,
            variables = new
            {
                id,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaById_Snaps_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MediaByIdQueries
{
    public const string GetItems = """
        query MediaByIdQuery($id: Int!) {
          mediaById(id: $id) {
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
