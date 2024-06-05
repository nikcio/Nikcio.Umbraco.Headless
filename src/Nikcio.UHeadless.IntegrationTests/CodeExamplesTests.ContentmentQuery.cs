using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests;
public partial class CodeExamplesTests
{
    private const string _contentmentExampleSnapshotPath = $"{SnapshotConstants.CodeExamplesBasePath}/ContentmentExample";

    [Theory]
    [InlineData("test-1", 1198, true)]
    public async Task ContentmentExampleQuery_Snaps_Async(
        string testCase,
        int id,
        bool expectSuccess)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentmentExampleSnapshotPath}/Snaps");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentmentExampleQueries.GetItems,
            variables = new
            {
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentmentExample_Snaps_{testCase}.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.Equal(expectSuccess, response.IsSuccessStatusCode);
    }
}

public static class ContentmentExampleQueries
{
    public const string GetItems = """
        query ContentmentQueryExample($id: Int!) {
          contentById(id: $id) {
            properties {
              ... on IContentmentPage {
                iconPicker {
                  value
                  model
                }
                listItems {
                  value
                  model
                }
                socialLinks {
                  value
                  model
                }
                numberInput {
                  value
                  model
                }
                textInput {
                  value
                  model
                }
                textboxList {
                  value
                  model
                }
                bytes {
                  value
                  model
                }
                editorNotes {
                  value
                  model
                }
                notes {
                  value
                  model
                }
                codeEditor {
                  value
                  model
                }
                dataList {
                  value
                  model
                }
                dataPicker {
                  value
                  model
                }
                __typename
              }
            }
          }
        }
        """;
}
