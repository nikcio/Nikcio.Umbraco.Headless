using System.Net.Http.Json;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _findMembersByRoleSnapshotPath = $"{SnapshotConstants.BasePath}/FindMembersByRole";

    [Theory]
    [InlineData("Member group 1", "", StringPropertyMatchType.Exact, 1, 0, true)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Exact, 1, 1, true)]
    [InlineData("Member group 1", "A", StringPropertyMatchType.Exact, 1, 1, true)]
    [InlineData("Member group 1", "B", StringPropertyMatchType.Exact, 1, 1, true)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Exact, 2, 1, true)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Exact, 1, 1000, true)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Exact, 1000, 1000, true)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Contains, 1, 5, true)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Contains, 0, 5, false)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Contains, -1, 5, false)]
    [InlineData("Member group 1", "", StringPropertyMatchType.Contains, 0, -1, false)]
    public async Task FindMembersByRoleQuery_Snaps_Async(
        string roleName,
        string usernameToMatch,
        StringPropertyMatchType matchType,
        int page,
        int pageSize,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_findMembersByRoleSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = FindMembersByRoleQueries.GetItems,
            variables = new
            {
                roleName,
                usernameToMatch,
                matchType = TestUtils.GetHotChocolateEnum(matchType.ToString()),
                page,
                pageSize,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"FindMembersByRole_Snaps_{roleName}_{usernameToMatch}_{matchType}_{page}_{pageSize}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class FindMembersByRoleQueries
{
    public const string GetItems = """
        query FindMembersByRoleQuery(
          $roleName: String!
          $usernameToMatch: String!
          $matchType: StringPropertyMatchType!
          $page: Int!
          $pageSize: Int!
        ) {
          findMembersByRole(
            roleName: $roleName
            usernameToMatch: $usernameToMatch
            matchType: $matchType
            page: $page
            pageSize: $pageSize
          ) {
            items {
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
            }
            page
            pageSize
            totalItems
            hasNextPage
            __typename
          }
        }
        """;
}
