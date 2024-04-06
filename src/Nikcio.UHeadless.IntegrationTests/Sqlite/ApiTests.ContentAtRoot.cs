using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _contentAtRootSnapshotPath = $"{SnapshotConstants.BasePath}/ContentAtRoot";

    [Theory]
    [InlineData(0, null)]
    [InlineData(1, null)]
    [InlineData(5, null)]
    [InlineData(10, null)]
    [InlineData(0, "en-us")]
    [InlineData(1, "en-us")]
    [InlineData(10, "en-us")]
    [InlineData(5, "da")]
    public async Task ContentAtRoot_Can_Get_FirstNodes_Async(int firstCount, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/FirstNodes");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetFirstNodesContentAtRoot($firstCount: Int!, $culture: String) {
                    contentAtRoot(first: $firstCount, order: { id: ASC}, culture: $culture) { 
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
                culture
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_GetFirstNodes_{firstCount}_{culture}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("en-us")]
    [InlineData("da")]
    public async Task ContentAtRoot_Can_Get_General_Async(string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralContentAtRoot($culture: String) {
                  contentAtRoot(culture: $culture) {
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
                culture
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_GetGeneral_{culture}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("en-us")]
    [InlineData("da")]
    public async Task ContentAtRoot_Can_Get_NodeId_Async(string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdContentAtRoot($culture: String) {
                    contentAtRoot(culture: $culture) { 
                        nodes {
                            id 
                        }
                    }
                }
                """,
            variables = new
            {
                culture
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_GetNodeId_{culture}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("en-us")]
    [InlineData("da")]
    public async Task ContentAtRoot_Can_Get_Preview_NodeId_Async(string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/PreviewNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPreviewNodeIdContentAtRoot($culture: String) {
                    contentAtRoot(preview: true, culture: $culture) { 
                        nodes {
                            id 
                        }
                    }
                }
                """,
            variables = new
            {
                culture
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_PreviewNodeId_{culture}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("en-us")]
    [InlineData("da")]
    public async Task ContentAtRoot_Can_Get_Properties_Async(string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesContentAtRoot($culture: String) {
                  contentAtRoot(culture: $culture) {
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
                culture
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_GetProperties_{culture}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
