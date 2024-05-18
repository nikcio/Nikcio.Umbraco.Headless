using System.Net.Http.Json;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _findMembersByUsernameSnapshotPath = $"{SnapshotConstants.BasePath}/FindMembersByUsername";

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
    public async Task FindMembersByUsernameQuery_Snaps_Async(
        string username,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByUsernameSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = FindMembersByUsernameQueries.GetItems,
            variables = new
            {
                username,
                matchType = matchType.ToString().ToUpperInvariant(),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"FindMembersByUsername_Snaps_{username}_{matchType}_{page}_{pageSize}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class FindMembersByUsernameQueries
{
    public const string GetItems = """
        query FindMembersByUsernameQuery(
          $username: String!,
          $matchType: StringPropertyMatchType!,
          $page: Int!,
          $pageSize: Int!
        ) {
          findMembersByUsername(
            username: $username, 
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
