using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _memberByUsernameSnapshotPath = $"{SnapshotConstants.BasePath}/MemberByUsername";

    [Theory]
    [InlineData("test-1", "user1", true)]
    [InlineData("test-2", "ARoleOne", true)]
    [InlineData("test-3", "member_2", true)]
    [InlineData("test-4", "Not_found", true)]
    public async Task MemberByUsernameQuery_Snaps_Async(
        string testCase,
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

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MemberByUsername_Snaps_{testCase}.snap";

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
            __typename
          }
        }
        """;
}
