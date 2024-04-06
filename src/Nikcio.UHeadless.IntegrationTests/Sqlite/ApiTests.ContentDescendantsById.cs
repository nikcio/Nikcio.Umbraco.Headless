using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _contentDescendantsByIdSnapshotPath = $"{SnapshotConstants.BasePath}/ContentDescendantsById";

    [Theory]
    [InlineData(1146, 0, null)]
    [InlineData(1146, 1, null)]
    [InlineData(1146, 5, null)]
    [InlineData(1146, 10, null)]
    [InlineData(1146, 0, "en-us")]
    [InlineData(1146, 1, "en-us")]
    [InlineData(1146, 10, "en-us")]
    [InlineData(1146, 5, "da")]
    public async Task ContentDescendantsById_Can_Get_FirstNodes_Async(int id, int firstCount, string? culture)
    {           
        var snapshotProvider = new SnapshotProvider($"{_contentAllSnapshotPath}/FirstNodes");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetFirstNodesContentDescendantsById($id: Int!, $firstCount: Int!, $culture: String) {
                    contentDescendantsById(id: $id, first: $firstCount, order: { id: ASC}, culture: $culture) { 
                        nodes {
                            id 
                        }
                        pageInfo {
                          hasNextPage
                        }
                    }
                }
                """,
            variables = new
            {
                firstCount,
                culture,
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAll_GetFirstNodes_{firstCount}_{culture}_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }


    [Theory]
    [InlineData(1146, null)]
    [InlineData(1152, null)]
    [InlineData(1159, null)]
    [InlineData(1163, null)]
    [InlineData(1175, null)]
    [InlineData(1147, null)]
    [InlineData(1153, null)]
    [InlineData(1148, null)]
    [InlineData(1154, null)]
    [InlineData(1162, "en-us")]
    [InlineData(1162, "da")]
    public async Task ContentDescendantsById_Can_Get_General_Async(int id, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByIdSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralContentDescendantsById($id: Int!, $culture: String) {
                  contentDescendantsById(id: $id, first: 50, culture: $culture) {
                    nodes {
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
                      redirect {
                        redirectUrl
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
                        redirect {
                          redirectUrl
                        }
                        sortOrder
                        templateId
                        url
                        urlSegment
                        absoluteUrl
                      }
                    }
                  }
                }
                """,
            variables = new
            {
                culture,
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsById_GetGeneral_{culture}_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(1146, null)]
    [InlineData(1152, null)]
    [InlineData(1159, null)]
    [InlineData(1163, null)]
    [InlineData(1175, null)]
    [InlineData(1147, null)]
    [InlineData(1153, null)]
    [InlineData(1148, null)]
    [InlineData(1154, null)]
    [InlineData(1162, "en-us")]
    [InlineData(1162, "da")]
    public async Task ContentDescendantsById_Can_Get_NodeId_Async(int id, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByIdSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdContentDescendantsById($id: Int!, $culture: String) {
                    contentDescendantsById(id: $id, first: 50, culture: $culture) { 
                        nodes {
                            id 
                        }
                    }
                }
                """,
            variables = new
            {
                culture,
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsById_GetNodeId_{culture}_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(1146, null)]
    [InlineData(1152, null)]
    [InlineData(1159, null)]
    [InlineData(1163, null)]
    [InlineData(1175, null)]
    [InlineData(1147, null)]
    [InlineData(1153, null)]
    [InlineData(1148, null)]
    [InlineData(1154, null)]
    [InlineData(1162, "en-us")]
    [InlineData(1162, "da")]
    public async Task ContentDescendantsById_Can_Get_Properties_Async(int id, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByIdSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesContentDescendantsById($id: Int!, $culture: String) {
                  contentDescendantsById(id: $id, first: 50, culture: $culture) {
                    nodes {
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
                }
                """ + Fragments.PropertyValues,
            variables = new
            {
                culture,
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsById_GetProperties_{culture}_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
