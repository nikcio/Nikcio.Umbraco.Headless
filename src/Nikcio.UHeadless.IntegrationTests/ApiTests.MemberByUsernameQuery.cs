using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _memberByUsernameSnapshotPath = $"{SnapshotConstants.BasePath}/MemberByUsername";

    [Theory]
    [InlineData("user1", true)]
    [InlineData("ARoleOne", true)]
    [InlineData("member_2", true)]
    [InlineData("Not_found", true)]
    public async Task MemberByUsernameQuery_Snaps_Async(
        string username,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByUsernameSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MemberByUsernameQueries.GetItems,
            variables = new
            {
                username,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MemberByUsername_Snaps_{username}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MemberByUsernameQueries
{
    public const string GetItems = """
        query MemberByUsernameQuery(
          $username: String!
        ) {
          memberByUsername(
            username: $username) {
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
