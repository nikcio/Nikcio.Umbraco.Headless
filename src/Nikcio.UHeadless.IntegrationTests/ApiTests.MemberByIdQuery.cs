using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _memberByIdSnapshotPath = $"{SnapshotConstants.BasePath}/MemberById";

    [Theory]
    [InlineData(1179, true)]
    [InlineData(1183, true)]
    [InlineData(1181, true)]
    [InlineData(1, true)]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    [InlineData(-1000, true)]
    public async Task MemberByIdQuery_Snaps_Async(
        int id,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByIdSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MemberByIdQueries.GetItems,
            variables = new
            {
                id,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MemberById_Snaps_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MemberByIdQueries
{
    public const string GetItems = """
        query MemberByIdQuery(
          $id: Int!
        ) {
          memberById(
            id: $id) {
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
