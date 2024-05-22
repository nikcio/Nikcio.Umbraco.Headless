using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _memberByEmailSnapshotPath = $"{SnapshotConstants.BasePath}/MemberByEmail";

    [Theory]
    [InlineData("user1@test.com", true)]
    [InlineData("role@test.com", true)]
    [InlineData("member2@test.com", true)]
    [InlineData("Not found", true)]
    public async Task MemberByEmailQuery_Snaps_Async(
        string email,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByEmailSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MemberByEmailQueries.GetItems,
            variables = new
            {
                email,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MemberByEmail_Snaps_{email}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MemberByEmailQueries
{
    public const string GetItems = """
        query MemberByEmailQuery(
          $email: String!
        ) {
          memberByEmail(
            email: $email) {
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
