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
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
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

        var response = await client.PostAsync("/graphql", request);

        var responseContent = await response.Content.ReadAsStringAsync();

        var snapshotName = $"MediaByContentType_GetFirstNodes_{firstCount}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaByContentType_Can_Get_General_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/GetGeneral");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
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

        var response = await client.PostAsync("/graphql", request);

        var responseContent = await response.Content.ReadAsStringAsync();

        var snapshotName = $"MediaByContentType_GetGeneral";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaByContentType_Can_Get_NodeId_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/GetNodeId");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
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

        var response = await client.PostAsync("/graphql", request);

        var responseContent = await response.Content.ReadAsStringAsync();

        var snapshotName = $"MediaByContentType_GetNodeId";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaByContentType_Can_Get_Properties_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByContentTypeSnapshotPath}/GetProperties");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
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

        var response = await client.PostAsync("/graphql", request);

        var responseContent = await response.Content.ReadAsStringAsync();

        var snapshotName = $"MediaByContentType_GetProperties";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }
}
