using System.Net.Http.Json;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _findMembersByUsernameSnapshotPath = $"{SnapshotConstants.BasePath}/FindMembersByUsername";

    [Theory]
    [InlineData("test-1", "user1", StringPropertyMatchType.Exact, 1, 0, false)]
    [InlineData("test-2", "user1", StringPropertyMatchType.Exact, 1, 1, true)]
    [InlineData("test-3", "user1", StringPropertyMatchType.Exact, 2, 1, true)]
    [InlineData("test-4", "user1", StringPropertyMatchType.Exact, 1, 1000, true)]
    [InlineData("test-5", "user1", StringPropertyMatchType.Exact, 1000, 1000, true)]
    [InlineData("test-6", "member", StringPropertyMatchType.Contains, 1, 5, true)]
    [InlineData("test-7", "member", StringPropertyMatchType.Contains, 0, 5, false)]
    [InlineData("test-8", "member", StringPropertyMatchType.Contains, -1, 5, false)]
    [InlineData("test-9", "member", StringPropertyMatchType.Contains, 0, -1, false)]
    public async Task FindMembersByUsernameQuery_Snaps_Async(
        string testCase,
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
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"FindMembersByUsername_Snaps_{testCase}.snap";

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
            __typename
          }
        }
        
        """;
}
