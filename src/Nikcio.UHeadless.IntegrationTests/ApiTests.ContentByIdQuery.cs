using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _contentByIdSnapshotPath = $"{SnapshotConstants.BasePath}/ContentById";

    [Theory]
    [InlineData(1146, "en-us", false, null, true)]
    [InlineData(1146, "da", false, null, true)]
    [InlineData(1146, null, false, null, true)]
    [InlineData(1149, "en-us", true, null, true)]
    [InlineData(1151, "da", null, null, true)]
    [InlineData(1165, null, null, null, true)]
    [InlineData(1175, null, null, null, true)]
    [InlineData(1176, null, null, null, true)]
    [InlineData(-1000, "en-us", false, null, true)]
    [InlineData(100_000_000, "en-us", false, null, true)]
    public async Task ContentByIdQuery_Snaps_Async(
        int id,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByIdSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentByIdQueries.GetItems,
            variables = new
            {
                id,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentById_Snaps_{id}_{culture}_{includePreview}_{segment}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentByIdQueries
{
    public const string GetItems = """
        query ContentByIdQuery(
          $id: Int!
          $culture: String
          $includePreview: Boolean
          $fallbacks: [PropertyFallback!]
          $segment: String
        ) {
          contentById(
            id: $id
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
