using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _mediaByIdSnapshotPath = $"{SnapshotConstants.BasePath}/MediaById";

    [Theory]
    [InlineData(1138)]
    [InlineData(1139)]
    [InlineData(1143)]
    [InlineData(1144)]
    public async Task MediaById_Can_Get_General_Async(int id)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByIdSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralMediaById($id: Int!) {
                  mediaById(id: $id) {
                    id
                    key
                    path
                    name
                    creatorId
                    writerId
                    properties {
                      alias
                    }
                    itemType
                    level
                    parent {
                      name
                    }
                    sortOrder
                    templateId
                    url
                    urlSegment
                    absoluteUrl
                    children {
                      name
                      creatorId
                      writerId
                      properties {
                        alias
                      }
                      itemType
                      level
                      parent {
                        name
                      }
                      sortOrder
                      templateId
                      url
                      urlSegment
                      absoluteUrl
                    }
                  }
                }
                """,
            variables = new
            {
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaById_GetGeneral_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(1138)]
    [InlineData(1139)]
    [InlineData(1143)]
    [InlineData(1144)]
    public async Task MediaById_Can_Get_NodeId_Async(int id)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByIdSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdMediaById($id: Int!) {
                    mediaById(id: $id) { 
                        id
                    }
                }
                """,
            variables = new
            {
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaById_GetNodeId_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(1138)]
    [InlineData(1139)]
    [InlineData(1143)]
    [InlineData(1144)]
    public async Task MediaById_Can_Get_Properties_Async(int id)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByIdSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesMediaById($id: Int!) {
                  mediaById(id: $id) {
                    properties {
                      alias
                      value {
                        ...propertyValues
                      }
                      editorAlias
                    }
                    children {
                      properties {
                        alias
                        value {
                          ...propertyValues
                        }
                      }
                    }
                  }
                }
                """ + Fragments.PropertyValues,
            variables = new
            {
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaById_GetProperties_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
