using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _contentByIdSnapshotPath = $"{SnapshotConstants.BasePath}/ContentById";

    [Theory]
    [InlineData("test-1", 1146, "en-US", false, null, true)]
    [InlineData("test-2", 1146, "da", false, null, true)]
    [InlineData("test-3", 1146, null, false, null, true)]
    [InlineData("test-4", 1149, "en-US", true, null, true)]
    [InlineData("test-5", 1151, "da", null, null, true)]
    [InlineData("test-6", 1165, null, null, null, true)]
    [InlineData("test-7", 1175, null, null, null, true)]
    [InlineData("test-8", 1176, null, null, null, true)]
    [InlineData("test-9", -1000, "en-US", false, null, true)]
    [InlineData("test-10", 100_000_000, "en-US", false, null, true)]
    public async Task ContentByIdQuery_Snaps_Async(
        string testCase,
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
                segment,
                baseUrl = "https://site-1.com"
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"ContentById_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentByIdQueries
{
    public const string GetItems = """
        query ContentByIdQuery(
          $baseUrl: String!
          $id: Int!
          $culture: String
          $includePreview: Boolean
          $fallbacks: [PropertyFallback!]
          $segment: String
        ) {
          contentById(
            id: $id
            inContext: {
              baseUrl: $baseUrl
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
