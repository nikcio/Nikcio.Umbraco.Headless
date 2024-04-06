﻿using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _mediaByGuidSnapshotPath = $"{SnapshotConstants.BasePath}/MediaByGuid";

    [Theory]
    [InlineData("25ba1577-a0c5-4329-8f32-9e9abe4a6d2d")]
    [InlineData("40e37a26-8570-47dd-b30a-a1bd391f603b")]
    [InlineData("d6355171-2e04-4b73-b11b-01f408ec242a")]
    [InlineData("b6113530-0ccf-4006-88d4-3dcd31c8297e")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task MediaByGuid_Can_Get_General_Async(string guid)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByGuidSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralMediaByGuid($guid: UUID!) {
                  mediaByGuid(id: $guid) {
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
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByGuid_GetGeneral_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("25ba1577-a0c5-4329-8f32-9e9abe4a6d2d")]
    [InlineData("40e37a26-8570-47dd-b30a-a1bd391f603b")]
    [InlineData("d6355171-2e04-4b73-b11b-01f408ec242a")]
    [InlineData("b6113530-0ccf-4006-88d4-3dcd31c8297e")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task MediaByGuid_Can_Get_NodeId_Async(string guid)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByGuidSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdMediaByGuid($guid: UUID!) {
                    mediaByGuid(id: $guid) { 
                        id
                    }
                }
                """,
            variables = new
            {
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByGuid_GetNodeId_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("25ba1577-a0c5-4329-8f32-9e9abe4a6d2d")]
    [InlineData("40e37a26-8570-47dd-b30a-a1bd391f603b")]
    [InlineData("d6355171-2e04-4b73-b11b-01f408ec242a")]
    [InlineData("b6113530-0ccf-4006-88d4-3dcd31c8297e")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Test")]
    public async Task MediaByGuid_Can_Get_Properties_Async(string guid)
    {
        var snapshotProvider = new SnapshotProvider($"{_mediaByGuidSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesMediaByGuid($guid: UUID!) {
                  mediaByGuid(id: $guid) {
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
                guid
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"MediaByGuid_GetProperties_{guid}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
