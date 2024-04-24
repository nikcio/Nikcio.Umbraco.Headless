using System.Net.Http.Json;

namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _contentAtRootSnapshotPath = $"{SnapshotConstants.BasePath}/ContentAtRoot";

    [Theory]
    [InlineData(0, 5, "en-us", false)]
    [InlineData(1, 5, "en-us", false)]
    [InlineData(0, 5, "en-us", true)]
    public async Task ContentAtRootQuery_Can_Get_Items_Async(int page, int pageSize, string culture, bool includePreview)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/CanGetItems");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = ContentAtRootQueries.GetItems,
            variables = new
            {
                page,
                pageSize,
                culture,
                includePreview
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentAtRoot_GetItems_{page}_{pageSize}_{culture}_{includePreview}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    //[Theory]
    //[InlineData(0, null)]
    //[InlineData(1, null)]
    //[InlineData(5, null)]
    //[InlineData(10, null)]
    //[InlineData(0, "en-us")]
    //[InlineData(1, "en-us")]
    //[InlineData(10, "en-us")]
    //[InlineData(5, "da")]
    //public async Task ContentAtRoot_Can_Get_FirstNodes_Async(int firstCount, string? culture)
    //{
    //    var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/FirstNodes");
    //    HttpClient client = _factory.CreateClient();

    //    using var request = JsonContent.Create(new
    //    {
    //        query = """
    //            query GetFirstNodesContentAtRoot($firstCount: Int!, $culture: String) {
    //                contentAtRoot(first: $firstCount, order: { id: ASC}, culture: $culture) { 
    //                    nodes {
    //                        id 
    //                    }
    //                    pageInfo {
    //                      hasNextPage
    //                    }
    //                }
    //            }
    //            """,
    //        variables = new
    //        {
    //            firstCount,
    //            culture
    //        }
    //    });

    //    HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

    //    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

    //    string snapshotName = $"ContentAtRoot_GetFirstNodes_{firstCount}_{culture}";

    //    await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
    //    Assert.True(response.IsSuccessStatusCode);
    //}

    //[Theory]
    //[InlineData(null)]
    //[InlineData("en-us")]
    //[InlineData("da")]
    //public async Task ContentAtRoot_Can_Get_General_Async(string? culture)
    //{
    //    var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/GetGeneral");
    //    HttpClient client = _factory.CreateClient();

    //    using var request = JsonContent.Create(new
    //    {
    //        query = """
    //            query GetGeneralContentAtRoot($culture: String) {
    //              contentAtRoot(culture: $culture) {
    //                nodes {
    //                  id
    //                  key
    //                  path
    //                  name
    //                  creatorId
    //                  writerId
    //                  properties {
    //                    alias
    //                  }
    //                  itemType
    //                  level
    //                  parent {
    //                    name
    //                  }
    //                  redirect {
    //                    redirectUrl
    //                  }
    //                  sortOrder
    //                  templateId
    //                  url
    //                  urlSegment
    //                  absoluteUrl
    //                  children {
    //                    name
    //                    creatorId
    //                    writerId
    //                    properties {
    //                      alias
    //                    }
    //                    itemType
    //                    level
    //                    parent {
    //                      name
    //                    }
    //                    redirect {
    //                      redirectUrl
    //                    }
    //                    sortOrder
    //                    templateId
    //                    url
    //                    urlSegment
    //                    absoluteUrl
    //                  }
    //                }
    //              }
    //            }
    //            """,
    //        variables = new
    //        {
    //            culture
    //        }
    //    });

    //    HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

    //    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

    //    string snapshotName = $"ContentAtRoot_GetGeneral_{culture}";

    //    await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
    //    Assert.True(response.IsSuccessStatusCode);
    //}

    //[Theory]
    //[InlineData(null)]
    //[InlineData("en-us")]
    //[InlineData("da")]
    //public async Task ContentAtRoot_Can_Get_NodeId_Async(string? culture)
    //{
    //    var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/GetNodeId");
    //    HttpClient client = _factory.CreateClient();

    //    using var request = JsonContent.Create(new
    //    {
    //        query = """
    //            query GetNodeIdContentAtRoot($culture: String) {
    //                contentAtRoot(culture: $culture) { 
    //                    nodes {
    //                        id 
    //                    }
    //                }
    //            }
    //            """,
    //        variables = new
    //        {
    //            culture
    //        }
    //    });

    //    HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

    //    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

    //    string snapshotName = $"ContentAtRoot_GetNodeId_{culture}";

    //    await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
    //    Assert.True(response.IsSuccessStatusCode);
    //}

    //[Theory]
    //[InlineData(null)]
    //[InlineData("en-us")]
    //[InlineData("da")]
    //public async Task ContentAtRoot_Can_Get_Preview_NodeId_Async(string? culture)
    //{
    //    var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/PreviewNodeId");
    //    HttpClient client = _factory.CreateClient();

    //    using var request = JsonContent.Create(new
    //    {
    //        query = """
    //            query GetPreviewNodeIdContentAtRoot($culture: String) {
    //                contentAtRoot(preview: true, culture: $culture) { 
    //                    nodes {
    //                        id 
    //                    }
    //                }
    //            }
    //            """,
    //        variables = new
    //        {
    //            culture
    //        }
    //    });

    //    HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

    //    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

    //    string snapshotName = $"ContentAtRoot_PreviewNodeId_{culture}";

    //    await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
    //    Assert.True(response.IsSuccessStatusCode);
    //}

    //[Theory]
    //[InlineData(null)]
    //[InlineData("en-us")]
    //[InlineData("da")]
    //public async Task ContentAtRoot_Can_Get_Properties_Async(string? culture)
    //{
    //    var snapshotProvider = new SnapshotProvider($"{_contentAtRootSnapshotPath}/GetProperties");
    //    HttpClient client = _factory.CreateClient();

    //    using var request = JsonContent.Create(new
    //    {
    //        query = """
    //            query GetPropertiesContentAtRoot($culture: String) {
    //              contentAtRoot(culture: $culture) {
    //                nodes {
    //                  properties {
    //                    alias
    //                    value {
    //                      ...propertyValues
    //                    }
    //                    editorAlias
    //                  }
    //                  children {
    //                    properties {
    //                      alias
    //                      value {
    //                        ...propertyValues
    //                      }
    //                    }
    //                  }
    //                }
    //              }
    //            }
    //            """ + Fragments.PropertyValues,
    //        variables = new
    //        {
    //            culture
    //        }
    //    });

    //    HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

    //    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

    //    string snapshotName = $"ContentAtRoot_GetProperties_{culture}";

    //    await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
    //    Assert.True(response.IsSuccessStatusCode);
    //}
}

public static class ContentAtRootQueries
{
    public const string GetItems = """
        query ContentAtRootQuery(
          $page: Int!
          $pageSize: Int!
          $culture: String!
          $includePreview: Boolean!
        ) {
          contentAtRoot(page: $page, pageSize: $pageSize)
            @inContext(culture: $culture, includePreview: $includePreview) {
            items {
              url(urlMode: ABSOLUTE)
              redirect {
                redirectUrl
                isPermanent
              }
              statusCode
              properties {
                ...typedProperties
                __typename
              }
              urlSegment
              name
              id
              key
              templateId
              updateDate
              parent {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                templateId
                updateDate
              }
              __typename
            }
            page
            pageSize
            totalItems
            hasNextPage
          }
        }
        """ + Fragments.TypedProperties;
}
