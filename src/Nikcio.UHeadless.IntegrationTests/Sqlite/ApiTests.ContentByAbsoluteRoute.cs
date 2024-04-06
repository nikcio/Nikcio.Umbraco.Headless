using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests
{
    private const string _contentByAbsoluteRouteSnapshotPath = $"{SnapshotConstants.BasePath}/ContentByAbsoluteRoute";

    [Theory]
    [InlineData("https://site-1.com", "/", null)]
    [InlineData("https://site-1.com", "/homepage", null)]
    [InlineData("https://site-1.com", "/page-1", null)]
    [InlineData("https://site-1.com", "/page-2", null)]
    [InlineData("https://site-1.com", "/collection-of-pages/block-list-page", null)]
    [InlineData("", "/no-domain-site", null)]
    [InlineData("", "/no-domain-homepage", null)]
    [InlineData("https://site-2.com", "/", null)]
    [InlineData("https://site-2.com", "/page-1", null)]
    [InlineData("https://site-culture.com", "/homepage", "en-us")]
    [InlineData("https://site-culture.dk", "/homepage", "da")]
    public async Task ContentByAbsoluteRoute_Can_Get_General_Async(string baseUrl, string route, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByAbsoluteRouteSnapshotPath}/GetGeneral");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetGeneralContentByAbsoluteRoute($baseUrl: String!, $route: String!, $culture: String) {
                  contentByAbsoluteRoute(baseUrl: $baseUrl, route: $route, culture: $culture) {
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
                baseUrl,
                route
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByAbsoluteRoute_GetGeneral_{culture}_{UrlEncoder.Default.Encode(route)}_{UrlEncoder.Default.Encode(baseUrl)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("https://site-1.com", "/", null)]
    [InlineData("https://site-1.com", "/homepage", null)]
    [InlineData("https://site-1.com", "/page-1", null)]
    [InlineData("https://site-1.com", "/page-2", null)]
    [InlineData("https://site-1.com", "/collection-of-pages/block-list-page", null)]
    [InlineData("", "/no-domain-site", null)]
    [InlineData("", "/no-domain-homepage", null)]
    [InlineData("https://site-2.com", "/", null)]
    [InlineData("https://site-2.com", "/page-1", null)]
    [InlineData("https://site-culture.com", "/homepage", "en-us")]
    [InlineData("https://site-culture.dk", "/homepage", "da")]
    public async Task ContentByAbsoluteRoute_Can_Get_NodeId_Async(string baseUrl, string route, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByAbsoluteRouteSnapshotPath}/GetNodeId");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetNodeIdContentByAbsoluteRoute($baseUrl: String!, $route: String!, $culture: String) {
                    contentByAbsoluteRoute(baseUrl: $baseUrl, route: $route, culture: $culture) { 
                        id
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

        string snapshotName = $"ContentByAbsoluteRoute_GetNodeId_{culture}_{UrlEncoder.Default.Encode(route)}_{UrlEncoder.Default.Encode(baseUrl)}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("https://site-1.com", "/", null)]
    [InlineData("https://site-1.com", "/homepage", null)]
    [InlineData("https://site-1.com", "/page-1", null)]
    [InlineData("https://site-1.com", "/page-2", null)]
    [InlineData("https://site-1.com", "/collection-of-pages/block-list-page", null)]
    [InlineData("", "/no-domain-site", null)]
    [InlineData("", "/no-domain-homepage", null)]
    [InlineData("https://site-2.com", "/", null)]
    [InlineData("https://site-2.com", "/page-1", null)]
    [InlineData("https://site-culture.com", "/homepage", "en-us")]
    [InlineData("https://site-culture.dk", "/homepage", "da")]
    public async Task ContentByAbsoluteRoute_Can_Get_Properties_Async(string baseUrl, string route, string? culture)
    {
        var snapshotProvider = new SnapshotProvider($"{_contentByAbsoluteRouteSnapshotPath}/GetProperties");
        HttpClient client = _factory.CreateClient();

        using var request = JsonContent.Create(new
        {
            query = """
                query GetPropertiesContentByAbsoluteRoute($baseUrl: String!, $route: String!, $culture: String) {
                  contentByAbsoluteRoute(baseUrl: $baseUrl, route: $route, culture: $culture) {
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
                baseUrl,
                route
            }
        });

        HttpResponseMessage response = await client.PostAsync("/graphql", request).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"ContentByAbsoluteRoute_GetProperties_{culture}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(route))}_{Convert.ToBase64String(Encoding.UTF8.GetBytes(baseUrl))}";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
