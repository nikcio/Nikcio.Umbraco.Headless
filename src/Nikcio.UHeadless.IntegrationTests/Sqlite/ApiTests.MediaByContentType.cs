using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _mediaByContentTypeSnapshotPath = $"{SnapshotConstants.BasePath}/MediaByContentType";

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task MediaByContentType_Can_Get_FirstNodes_Async(int firstCount)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/FirstNodes");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetFirstNodesMediaByContentType($firstCount: Int!) {
                    mediaByContentType(contentType: "image", first: $firstCount, order: { id: ASC}) { 
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
                firstCount
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_GetFirstNodes_{firstCount}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaByContentType_Can_Get_General_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralMediaByContentType {
                  mediaByContentType(contentType: "image") {
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
                }
                """,
            variables = new
            {
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_GetGeneral";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaByContentType_Can_Get_NodeId_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdMediaByContentType {
                    mediaByContentType(contentType: "image") { 
                        nodes {
                            id 
                        }
                    }
                }
                """,
            variables = new
            {
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_GetNodeId";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaByContentType_Can_Get_Properties_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesMediaByContentType {
                  mediaByContentType(contentType: "image") {
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
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByContentType_GetProperties";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
