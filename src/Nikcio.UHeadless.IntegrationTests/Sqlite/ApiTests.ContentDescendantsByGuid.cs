using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _contentDescendantsByGuidSnapshotPath = $"{SnapshotConstants.BasePath}/ContentDescendantsByGuid";

    [Theory]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 0, null)]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 1, null)]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 5, null)]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 10, null)]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 0, "en-us")]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 1, "en-us")]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 10, "en-us")]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", 5, "da")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task ContentDescendantsByGuid_Can_Get_FirstNodes_Async(string guid, int firstCount, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAllSnapshotPath}/FirstNodes");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetFirstNodesContentDescendantsByGuid($guid: UUID!, $firstCount: Int!, $culture: String) {
                    contentDescendantsByGuid(id: $guid, first: $firstCount, order: { id: ASC}, culture: $culture) { 
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
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAll_GetFirstNodes_{firstCount}_{culture}_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }


    [Theory]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", null)]
    [InlineData("ca69a30f-bf47-4acf-b31b-556c585d204b", null)]
    [InlineData("3686dd5d-f689-4e3b-ace6-b6d4e8b06165", null)]
    [InlineData("f578f70d-13f4-444a-baa7-a1a3c1bc1666", null)]
    [InlineData("24099846-1261-4ee7-b2cf-223424767bdb", null)]
    [InlineData("cb9726b8-2307-4a01-9b39-7c2850c4d19a", null)]
    [InlineData("fa22ad17-c823-4e1f-a87f-7597d0ed75b2", null)]
    [InlineData("dcf14fa0-dfc3-4108-8c9b-a4e9f77c0c29", null)]
    [InlineData("4706632f-6641-4b60-a6d4-31230172b197", null)]
    [InlineData("3fa4a316-249b-487f-8d9e-007581f7b748", "en-us")]
    [InlineData("3fa4a316-249b-487f-8d9e-007581f7b748", "da")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task ContentDescendantsByGuid_Can_Get_General_Async(string guid, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByGuidSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralContentDescendantsByGuid($guid: UUID!, $culture: String) {
                  contentDescendantsByGuid(id: $guid, first: 50, culture: $culture) {
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
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByGuid_GetGeneral_{culture}_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", null)]
    [InlineData("ca69a30f-bf47-4acf-b31b-556c585d204b", null)]
    [InlineData("3686dd5d-f689-4e3b-ace6-b6d4e8b06165", null)]
    [InlineData("f578f70d-13f4-444a-baa7-a1a3c1bc1666", null)]
    [InlineData("24099846-1261-4ee7-b2cf-223424767bdb", null)]
    [InlineData("cb9726b8-2307-4a01-9b39-7c2850c4d19a", null)]
    [InlineData("fa22ad17-c823-4e1f-a87f-7597d0ed75b2", null)]
    [InlineData("dcf14fa0-dfc3-4108-8c9b-a4e9f77c0c29", null)]
    [InlineData("4706632f-6641-4b60-a6d4-31230172b197", null)]
    [InlineData("3fa4a316-249b-487f-8d9e-007581f7b748", "en-us")]
    [InlineData("3fa4a316-249b-487f-8d9e-007581f7b748", "da")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task ContentDescendantsByGuid_Can_Get_NodeId_Async(string guid, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByGuidSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdContentDescendantsByGuid($guid: UUID!, $culture: String) {
                    contentDescendantsByGuid(id: $guid, first: 50, culture: $culture) { 
                        nodes {
                            id 
                        }
                    }
                }
                """,
            variables = new
            {
                culture,
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByGuid_GetNodeId_{culture}_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf", null)]
    [InlineData("ca69a30f-bf47-4acf-b31b-556c585d204b", null)]
    [InlineData("3686dd5d-f689-4e3b-ace6-b6d4e8b06165", null)]
    [InlineData("f578f70d-13f4-444a-baa7-a1a3c1bc1666", null)]
    [InlineData("24099846-1261-4ee7-b2cf-223424767bdb", null)]
    [InlineData("cb9726b8-2307-4a01-9b39-7c2850c4d19a", null)]
    [InlineData("fa22ad17-c823-4e1f-a87f-7597d0ed75b2", null)]
    [InlineData("dcf14fa0-dfc3-4108-8c9b-a4e9f77c0c29", null)]
    [InlineData("4706632f-6641-4b60-a6d4-31230172b197", null)]
    [InlineData("3fa4a316-249b-487f-8d9e-007581f7b748", "en-us")]
    [InlineData("3fa4a316-249b-487f-8d9e-007581f7b748", "da")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task ContentDescendantsByGuid_Can_Get_Properties_Async(string guid, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByGuidSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesContentDescendantsByGuid($guid: UUID!, $culture: String) {
                  contentDescendantsByGuid(id: $guid, first: 50, culture: $culture) {
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
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByGuid_GetProperties_{culture}_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
