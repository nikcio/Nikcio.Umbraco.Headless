using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _contentByTagSnapshotPath = $"{SnapshotConstants.BasePath}/ContentByTag";

    [Theory]
    [InlineData("test-1", "normal", null, "en-us", false, null, true)]
    [InlineData("test-2", "normal", null, "en-us", true, null, true)]
    [InlineData("test-3", "normal", null, "da", false, null, true)]
    public async Task ContentByTagQuery_Snaps_Async(
        string testCase,
        string tag,
        string? tagGroup,
        string? culture,
        bool? includePreview,
        string? segment,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByTagSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentByTagQueries.GetItems,
            variables = new
            {
                tag,
                tagGroup,
                culture,
                includePreview,
                segment
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByTag_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentByTagQueries
{
    public const string GetItems = """
        query ContentByTagQuery(
          $tag: String!,
          $tagGroup: String,
          $culture: String
          $includePreview: Boolean
          $fallbacks: [PropertyFallback!]
          $segment: String
        ) {
          contentByTag(
            tag: $tag,
            tagGroup: $tagGroup
            inContext: {
              culture: $culture
              includePreview: $includePreview
              fallbacks: $fallbacks
              segment: $segment
            }
          ) {
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
