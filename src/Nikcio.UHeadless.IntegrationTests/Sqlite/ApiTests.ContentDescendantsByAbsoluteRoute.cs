using System.Net.Http.Json;
using System.Text;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _contentDescendantsByAbsoluteRouteSnapshotPath = $"{SnapshotConstants.BasePath}/ContentDescendantsByAbsoluteRoute";

    [Theory]
    [InlineData("https://site-1.com/", "/", 0, null)]
    [InlineData("https://site-1.com/", "/", 1, null)]
    [InlineData("https://site-1.com/", "/", 2, null)]
    [InlineData("https://site-1.com/", "/", 5, null)]
    [InlineData("https://site-1.com/", "/", 10, null)]
    [InlineData("https://site-culture.com/", "/", 0, "en-us")]
    [InlineData("https://site-culture.com/", "/", 1, "en-us")]
    [InlineData("https://site-culture.com/", "/", 10, "en-us")]
    public async Task ContentDescendantsByAbsoluteRoute_Can_Get_FirstNodes_Async(string baseUrl, string route, int firstCount, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByAbsoluteRouteSnapshotPath}/FirstNodes");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetFirstNodesContentDescendantsByAbsoluteRoute($baseUrl: String!, $route: String!, $firstCount: Int!, $culture: String) {
                    contentDescendantsByAbsoluteRoute(baseUrl: $baseUrl, route: $route, first: $firstCount, order: { id: ASC}, culture: $culture) { 
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
                baseUrl,
                route
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByAbsoluteRoute_GetFirstNodes_{firstCount}_{culture}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("https://site-1.com", "/", null)]
    [InlineData("https://site-1.com", "/collection-of-pages", null)]
    [InlineData("", "/no-domain-site", null)]
    [InlineData("https://site-2.com", "/", null)]
    [InlineData("https://site-culture.com", "/", "en-us")]
    [InlineData("https://site-culture.dk", "/", "da")]
    public async Task ContentDescendantsByAbsoluteRoute_Can_Get_General_Async(string baseUrl, string route, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByAbsoluteRouteSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralContentDescendantsByAbsoluteRoute($baseUrl: String!, $route: String!, $culture: String) {
                  contentDescendantsByAbsoluteRoute(baseUrl: $baseUrl, route: $route, first: 50, culture: $culture) {
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
                route,
                baseUrl
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByAbsoluteRoute_GetGeneral_{culture}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("https://site-1.com", "/", null)]
    [InlineData("https://site-1.com", "/collection-of-pages", null)]
    [InlineData("", "/no-domain-site", null)]
    [InlineData("https://site-2.com", "/", null)]
    [InlineData("https://site-culture.com", "/", "en-us")]
    [InlineData("https://site-culture.dk", "/", "da")]
    public async Task ContentDescendantsByAbsoluteRoute_Can_Get_NodeId_Async(string baseUrl, string route, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByAbsoluteRouteSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdContentDescendantsByAbsoluteRoute($baseUrl: String!, $route: String!, $culture: String) {
                    contentDescendantsByAbsoluteRoute(baseUrl: $baseUrl, route: $route, first: 50, culture: $culture) { 
                        nodes {
                            id 
                        }
                    }
                }
                """,
            variables = new
            {
                culture,
                baseUrl,
                route
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByAbsoluteRoute_GetNodeId_{culture}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("https://site-1.com", "/", null)]
    [InlineData("https://site-1.com", "/collection-of-pages", null)]
    [InlineData("", "/no-domain-site", null)]
    [InlineData("https://site-2.com", "/", null)]
    [InlineData("https://site-culture.com", "/", "en-us")]
    [InlineData("https://site-culture.dk", "/", "da")]
    public async Task ContentDescendantsByAbsoluteRoute_Can_Get_Properties_Async(string baseUrl, string route, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentDescendantsByAbsoluteRouteSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesContentDescendantsByAbsoluteRoute($baseUrl: String!, $route: String!, $culture: String) {
                  contentDescendantsByAbsoluteRoute(baseUrl: $baseUrl, route: $route, first: 50, culture: $culture) {
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
                baseUrl,
                route
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentDescendantsByAbsoluteRoute_GetProperties_{culture}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
