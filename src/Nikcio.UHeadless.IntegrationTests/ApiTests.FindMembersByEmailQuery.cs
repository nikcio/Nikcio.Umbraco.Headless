using System.Net.Http.Json;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _findMembersByEmailSnapshotPath = $"{SnapshotConstants.BasePath}/FindMembersByEmail";

    [Theory]
    [InlineData("test-1", "user1@test.com", StringPropertyMatchType.Exact, 1, 0, false)]
    [InlineData("test-2", "user1@test.com", StringPropertyMatchType.Exact, 1, 1, true)]
    [InlineData("test-3", "user1@test.com", StringPropertyMatchType.Exact, 2, 1, true)]
    [InlineData("test-4", "user1@test.com", StringPropertyMatchType.Exact, 1, 1000, true)]
    [InlineData("test-5", "user1@test.com", StringPropertyMatchType.Exact, 1000, 1000, true)]
    [InlineData("test-6", "member", StringPropertyMatchType.Contains, 1, 5, true)]
    [InlineData("test-7", "member", StringPropertyMatchType.Contains, 0, 5, false)]
    [InlineData("test-8", "member", StringPropertyMatchType.Contains, -1, 5, false)]
    [InlineData("test-9", "member", StringPropertyMatchType.Contains, 0, -1, false)]
    public async Task FindMembersByEmailQuery_Snaps_Async(
        string testCase,
        string email,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByEmailSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = FindMembersByEmailQueries.GetItems,
            variables = new
            {
                email,
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"FindMembersByEmail_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class FindMembersByEmailQueries
{
    public const string GetItems = """
        query FindMembersByEmailQuery(
          $email: String!,
          $matchType: StringPropertyMatchType!,
          $page: Int!,
          $pageSize: Int!
        ) {
          findMembersByEmail(
            email: $email, 
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
