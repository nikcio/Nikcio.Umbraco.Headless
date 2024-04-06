using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _contentByIdSnapshotPath = $"{SnapshotConstants.BasePath}/ContentById";

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
    public async Task ContentById_Can_Get_General_Async(int id, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByIdSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralContentById($id: Int!, $culture: String) {
                  contentById(id: $id, culture: $culture) {
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
                """,
            variables = new
            {
                culture,
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentById_GetGeneral_{culture}_{id}";

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
    public async Task ContentById_Can_Get_NodeId_Async(int id, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByIdSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdContentById($id: Int!, $culture: String) {
                    contentById(id: $id, culture: $culture) { 
                        id
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

        string snapshotName = $"ContentById_GetNodeId_{culture}_{id}";

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
    public async Task ContentById_Can_Get_Properties_Async(int id, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByIdSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesContentById($id: Int!, $culture: String) {
                  contentById(id: $id, culture: $culture) {
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
                culture,
                id
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentById_GetProperties_{culture}_{id}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
