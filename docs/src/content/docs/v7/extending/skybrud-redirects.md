---
title: Integrating Skybrud.Umbraco.Redirects in Nikcio.UHeadless
description: Learn how to integrate Skybrud.Umbraco.Redirects in Nikcio.UHeadless.
---

The Nikcio.UHeadless package provides a way to integrate the Skybrud.Umbraco.Redirects package in your Umbraco Headless project. This allows you to query for content like you would usually do but with the added benefit of getting redirect information from SKybrud.Umbraco.Redirects.

## Example

:::note
This example is based on `Skybrud.Umbraco.Redirects` version `13.0.4`
:::

### Step 1: Install the Skybrud.Umbraco.Redirects package

First, you need to install the `Skybrud.Umbraco.Redirects` package. You can do this by running the following command in the Package Manager Console:

```bash
Install-Package Skybrud.Umbraco.Redirects
```

### Step 2: Create a query class

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Services;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class SkybrudRedirectsExampleQuery : ContentByRouteQuery
{
    [GraphQLName("skybrudRedirectsExampleQuery")]
    public override Task<ContentItem?> ContentByRouteAsync(
        IResolverContext resolverContext,
        [GraphQLDescription("The route to fetch. Example '/da/frontpage/'.")] string route,
        [GraphQLDescription("The base url for the request. Example: 'https://localhost:4000'. Default is the current domain")] string baseUrl = "",
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        return base.ContentByRouteAsync(resolverContext, route, baseUrl, inContext);
    }

    protected override async Task<ContentItem?> CreateContentItemFromRouteAsync(IResolverContext resolverContext, string route, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentNullException.ThrowIfNull(baseUrl);

        IRedirectsService redirectService = resolverContext.Service<IRedirectsService>();

        var uri = new Uri($"{baseUrl.TrimEnd('/')}{route}");
        IRedirect? redirect = redirectService.GetRedirectByUri(uri);

        if (redirect != null)
        {
            IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

            string redirectUrl = redirect.Destination.FullUrl;

            if (redirect.ForwardQueryString)
            {
                redirectUrl = redirectUrl.TrimEnd('/') + uri.Query;
            }

            return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
            {
                PublishedContent = null,
                ResolverContext = resolverContext,
                Redirect = new()
                {
                    IsPermanent = redirect.IsPermanent,
                    RedirectUrl = redirectUrl,
                },
                StatusCode = redirect.IsPermanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status307TemporaryRedirect,
            });
        }

        return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
    }
}
```

This query class extends the `ContentByRouteQuery` query and overrides the `CreateContentItemFromRouteAsync` method. This method is called when a content item is fetched by route. In this method, we get the `IRedirectsService` from the `IResolverContext` and fetch the redirect for the route. If a redirect is found, we create a new `ContentItem` with the redirect information.

### Step 3: Add the query to the UHeadless configuration

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<SkybrudRedirectsExampleQuery>();
})
```

### Step 4: Query the content

Now you can query the content by route and get the redirect information:

```graphql
query {
  skybrudRedirectsExampleQuery(baseUrl: "", route: "/") {
    redirect {
      isPermanent
      redirectUrl
    }
  }
}
```

This will return the redirect information if a redirect is found for the route. If no redirect is found, the content item will be fetched as usual.