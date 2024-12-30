using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _memberByGuidSnapshotPath = $"{SnapshotConstants.BasePath}/MemberByGuid";

    [Theory]
    [InlineData("test-1", "91eecdea-fc1e-408d-ac41-545ceefa3dc5", true)]
    [InlineData("test-2", "c0baa0a3-2f2a-4093-a40e-96f579f43912", true)]
    [InlineData("test-3", "1ee7e46f-372e-4cf8-b7f4-c8c87abc6ce2", true)]
    [InlineData("test-4", "00000000-0000-0000-0000-000000000001", true)]
    public async Task MemberByGuidQuery_Snaps_Async(
        string testCase,
        Guid key,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_memberByGuidSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = MemberByGuidQueries.GetItems,
            variables = new
            {
                key,
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request, TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"MemberByGuid_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class MemberByGuidQueries
{
    public const string GetItems = """
        query MemberByGuidQuery(
          $key: UUID!
        ) {
          memberByGuid(
            id: $key) {
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
