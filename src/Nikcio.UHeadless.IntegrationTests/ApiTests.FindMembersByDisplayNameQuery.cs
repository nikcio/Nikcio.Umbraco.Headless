using System.Net.Http.Json;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _findMembersByDisplayNameSnapshotPath = $"{SnapshotConstants.BasePath}/FindMembersByDisplayName";

    [Theory]
    [InlineData("user1", StringPropertyMatchType.Exact, 1, 0, false)]
    [InlineData("user1", StringPropertyMatchType.Exact, 1, 1, true)]
    [InlineData("user1", StringPropertyMatchType.Exact, 2, 1, true)]
    [InlineData("user1", StringPropertyMatchType.Exact, 1, 1000, true)]
    [InlineData("user1", StringPropertyMatchType.Exact, 1000, 1000, true)]
    [InlineData("member", StringPropertyMatchType.Contains, 1, 5, true)]
    [InlineData("member", StringPropertyMatchType.Contains, 0, 5, false)]
    [InlineData("member", StringPropertyMatchType.Contains, -1, 5, false)]
    [InlineData("member", StringPropertyMatchType.Contains, 0, -1, false)]
    public async Task FindMembersByDisplayNameQuery_Snaps_Async(
        string displayName,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByDisplayNameSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = FindMembersByDisplayNameQueries.GetItems,
            variables = new
            {
                displayName,
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"FindMembersByDisplayName_Snaps_{displayName}_{matchType}_{page}_{pageSize}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class FindMembersByDisplayNameQueries
{
    public const string GetItems = """
        query FindMembersByDisplayNameQuery(
          $displayName: String!,
          $matchType: StringPropertyMatchType!,
          $page: Int!,
          $pageSize: Int!
        ) {
          findMembersByDisplayName(
            displayName: $displayName, 
            matchType: $matchType, 
            page: $page, 
            pageSize: $pageSize) {
            properties {
              ... on ITestMember {
                nestedContent {
                  members {
                    name
                    id
                    key 
                  }
                  model
                }
                umbracoMemberComments {
                  value
                   model
                }
              }
              ... on IMember {
                umbracoMemberComments {
                  value
                  model
                }
                umbracoMemberFailedPasswordAttempts {
                  value
                  model
                }
                umbracoMemberApproved {
                  value
                  model
                }
                umbracoMemberLockedOut {
                  value
                  model
                }
                umbracoMemberLastLockoutDate {
                  value
                  model
                }
                umbracoMemberLastLogin {
                  value
                  model
                }
                umbracoMemberLastPasswordChangeDate {
                  value
                  model
                }
              }
            }
            name
            id
            key
            templateId
            parent {
              name
              id
              key
              templateId
            } 
            __typename
          }
        }
        """;
}
