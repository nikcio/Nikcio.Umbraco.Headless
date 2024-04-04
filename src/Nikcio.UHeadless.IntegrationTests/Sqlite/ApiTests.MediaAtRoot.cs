using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _mediaAtRootSnapshotPath = $"{SnapshotConstants.BasePath}/MediaAtRoot";

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task MediaAtRoot_Can_Get_FirstNodes_Async(int firstCount)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaAtRootSnapshotPath}/FirstNodes");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
        {
            query = """
                query GetFirstNodesMediaAtRoot($firstCount: Int!) {
                    mediaAtRoot(first: $firstCount, order: { id: ASC}) { 
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

        var snapshotName = $"MediaAtRoot_GetFirstNodes_{firstCount}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaAtRoot_Can_Get_General_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaAtRootSnapshotPath}/GetGeneral");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralMediaAtRoot {
                  mediaAtRoot {
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

        var snapshotName = $"MediaAtRoot_GetGeneral";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaAtRoot_Can_Get_NodeId_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaAtRootSnapshotPath}/GetNodeId");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdMediaAtRoot {
                    mediaAtRoot { 
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

        var snapshotName = $"MediaAtRoot_GetNodeId";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task MediaAtRoot_Can_Get_Properties_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaAtRootSnapshotPath}/GetProperties");
        var client = _factory.CreateClient();

        var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesMediaAtRoot {
                  mediaAtRoot {
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

        var snapshotName = $"MediaAtRoot_GetProperties";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent);
        Assert.True(response.IsSuccessStatusCode);
    }
}
