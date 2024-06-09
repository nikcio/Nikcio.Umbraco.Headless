---
title: Integrating UrlTracker in Nikcio.UHeadless
description: Learn how to integrate UrlTracker in Nikcio.UHeadless.
---

The Nikcio.UHeadless package provides a way to integrate the UrlTracker package into your UHeadless project. This allows you to use the UrlTracker package for redirects in your UHeadless project.

## Example

:::note
This example is based on `UrlTracker` version `13.2.0-alpha0003`
:::

### Step 1: Install the UrlTracker package

First, you need to install the `UrlTracker` package. You can do this by running the following command in the Package Manager Console:

```bash
Install-Package UrlTracker
```

### Step 2: Create a query class

```csharp
using System.Text.RegularExpressions;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.Options;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlTracker.Core;
using UrlTracker.Core.Intercepting.Models;
using UrlTracker.Core.Models;
using UrlTracker.Web.Processing;

namespace Code.Examples.Headless.UrlTrackerExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class UrlTrackerExampleQuery : ContentByRouteQuery
{
    [GraphQLName("urlTrackerExampleQuery")]
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

        IRequestInterceptFilterCollection requestInterceptFilters = resolverContext.Service<IRequestInterceptFilterCollection>();

        var requestedUrl = Url.Parse($"{baseUrl.TrimEnd('/')}{route}");

        if (!await requestInterceptFilters.EvaluateCandidateAsync(requestedUrl).ConfigureAwait(false))
        {
            return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
        }

        IInterceptService interceptService = resolverContext.Service<IInterceptService>();

        IIntercept intercept = await interceptService.GetAsync(requestedUrl).ConfigureAwait(false);

        if (intercept.Info is not Redirect redirect)
        {
            return await base.CreateContentItemFromRouteAsync(resolverContext, route, baseUrl).ConfigureAwait(false);
        }

        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        string? redirectUrl = redirect.Target switch
        {

            UrlTargetStrategy target => GetUrl(resolverContext, redirect, target, requestedUrl),
            ContentPageTargetStrategy target => GetUrl(resolverContext, redirect, target, requestedUrl),
            _ => throw new NotImplementedException(),
        };

        return contentItemRepository.GetContentItem(new ContentItem.CreateCommand()
        {
            PublishedContent = null,
            ResolverContext = resolverContext,
            Redirect = new()
            {
                IsPermanent = redirect.Permanent,
                RedirectUrl = redirectUrl,
            },
            StatusCode = redirectUrl == null ? StatusCodes.Status410Gone : GetStatusCode(redirect),
        });
    }

    private static int GetStatusCode(Redirect redirect)
    {
        return redirect.Permanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status307TemporaryRedirect;
    }

    private static string? GetUrl(IResolverContext resolverContext, Redirect redirect, UrlTargetStrategy target, Url requestedUrl)
    {
        string urlString = target.Url;

        if (redirect.Source is RegexSourceStrategy regexsource)
        {
            urlString = Regex.Replace((requestedUrl.Path + requestedUrl.Query).TrimStart('/'), regexsource.Value, urlString, RegexOptions.None, TimeSpan.FromMilliseconds(100));
        }

        var url = Url.Parse(urlString);

        if (redirect.RetainQuery)
        {
            url.Query = requestedUrl.Query;
        }

        if (url.AvailableUrlTypes.Contains(UrlTracker.Core.Models.UrlType.Absolute))
        {
            RequestHandlerSettings requestHandlerSettingsValue = resolverContext.Service<IOptions<RequestHandlerSettings>>().Value;
            return url.ToString(UrlTracker.Core.Models.UrlType.Absolute, requestHandlerSettingsValue.AddTrailingSlash);
        }
        else
        {
            return url.ToString();
        }
    }

    private static string? GetUrl(IResolverContext resolverContext, Redirect redirect, ContentPageTargetStrategy target, Url requestedUrl)
    {
        if (target.Content is null)
        {
            return null;
        }

        IPublishedUrlProvider publishedUrlProvider = resolverContext.Service<IPublishedUrlProvider>();
        var url = Url.Parse(target.Content.Url(publishedUrlProvider, target.Culture.DefaultIfNullOrWhiteSpace(null), UrlMode.Absolute));

        if (redirect.RetainQuery)
        {
            url.Query = requestedUrl.Query;
        }

        if (url.AvailableUrlTypes.Contains(UrlTracker.Core.Models.UrlType.Absolute))
        {
            RequestHandlerSettings requestHandlerSettingsValue = resolverContext.Service<IOptions<RequestHandlerSettings>>().Value;
            return url.ToString(UrlTracker.Core.Models.UrlType.Absolute, requestHandlerSettingsValue.AddTrailingSlash);
        }
        else
        {
            return url.ToString();
        }
    }
}
```

This query class extends the `ContentByRouteQuery` query and overrides the `CreateContentItemFromRouteAsync` method. This method is called when a content item is fetched by route. In this method, we get the `IInterceptService` from the `IResolverContext` and fetch the redirect for the route. If a redirect is found, we create a new `ContentItem` with the redirect information.

### Step 3: Register the query class

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<UrlTrackerExampleQuery>();
})
```

### Step 4: Query the content

Now you can query the content by route and get the redirect information:

```graphql
query {
  urlTrackerExampleQuery(baseUrl: "", route: "/") {
    redirect {
      isPermanent
      redirectUrl
    }
  }
}
```

## Track error status codes

The UrlTracker package also allows you to track error status codes to get recommendations about missing redirects. You can create a mutation to track these error status codes from your frontend like this:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Authorization;
using UrlTracker.Middleware.Background;

namespace Code.Examples.Headless.UrlTrackerExample;

[ExtendObjectType(typeof(HotChocolateMutationObject))]
public class TrackErrorStatusCodeMutation : IGraphQLMutation
{
    public const string PolicyName = "TrackErrorStatusCode";

    public const string ClaimValue = "track.error.statuscode.mutation";

    [GraphQLIgnore]
    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorizationBuilder().AddPolicy(PolicyName, policy =>
        {
            if (options.DisableAuthorization)
            {
                policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
                return;
            }

            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadless);

            policy.RequireAuthenticatedUser();

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue);
        });
    }

    public async Task<TrackErrorStatusCodeResponse> TrackErrorStatusCodeAsync(
        IResolverContext resolverContext,
        [GraphQLDescription("Status code of the client error.")] int statusCode,
        [GraphQLDescription("The URL that generated the client error.")] string url,
        [GraphQLDescription("The time and date at which the client error was generated")] DateTime timestamp,
        [GraphQLDescription("The URL from which the current URL is requested")] string? referrer)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        ILogger<TrackErrorStatusCodeMutation> logger = resolverContext.Service<ILogger<TrackErrorStatusCodeMutation>>();
        switch (statusCode)
        {
            case StatusCodes.Status404NotFound:
                IClientErrorProcessorQueue clientErrorProcessorQueue = resolverContext.Service<IClientErrorProcessorQueue>();
                await clientErrorProcessorQueue.WriteAsync(new ClientErrorProcessorItem(url, timestamp, referrer)).ConfigureAwait(false);
                break;
            case StatusCodes.Status500InternalServerError:
                logger.LogError("Internal server error occurred at {Timestamp} for URL {Url} with referrer {Referrer}", timestamp, url, referrer);
                break;
            default:
                logger.LogWarning("Client error occurred at {Timestamp} for URL {Url} with referrer {Referrer} and status code {StatusCode}", timestamp, url, referrer, statusCode);
                break;
        }
        
        return new TrackErrorStatusCodeResponse
        {
            Success = true
        };
    }
}

public sealed class TrackErrorStatusCodeResponse
{
    public required bool Success { get; init; }
}
```

This mutation class allows you to track error status codes from your frontend. You can use this mutation to track client errors like `404` and `500` errors. You can also track the time and date at which the client error was generated and the URL from which the current URL is requested.

### Register the mutation class

```csharp
.AddUHeadless(options =>
{
    options.AddMutation<TrackErrorStatusCodeMutation>();
})
```

### Query the mutation

Now you can query the mutation to track the error status codes:

```graphql
mutation {
  trackErrorStatusCode(statusCode: 404, url: "https://my-website.com/page-not-found", timestamp: "2022-01-01T00:00:00Z", referrer: "https://my-website.com") {
    success
  }
}
```
