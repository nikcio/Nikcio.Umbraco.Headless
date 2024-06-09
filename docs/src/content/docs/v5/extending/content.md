---
title: Extending Content Data Structures in Nikcio.UHeadless
description: Learn how to extend the content model in Nikcio.UHeadless.
---

Nikcio.UHeadless provides flexibility to extend and replace content data structures to accommodate your specific needs. This documentation outlines some examples of how you can extend the content data structures.

## Example custom content model extending the existing content model:

1. Create your content model by inheriting from `Nikcio.UHeadless.Defaults.ContentItems.ContentItem`:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;

/// <summary>
/// This example demonstrates how to create a custom content item with custom properties and methods.
/// </summary>
/// <remarks>
/// The <see cref="IResolverContext"/> can be used to resolve services from the DI container like you normally would with dependency injection.
/// It's important to contain any logic to the specific property or method within the property or method itself if possiable.
/// As GraphQL will only call the properties or methods that are requestedm and may not call all of them.
/// </remarks>
[GraphQLName("CustomContentItemExampleContentItem")]
public class ContentItem : Nikcio.UHeadless.Defaults.ContentItems.ContentItem
{
    public ContentItem(CreateCommand command) : base(command)
    {
    }

    public string? CustomProperty => "Custom value";

    public string? CustomMethod()
    {
        return "Custom method";
    }

    public string? CustomMethodWithParameter(string? parameter)
    {
        return $"Custom method with parameter: {parameter}";
    }

    public string? CustomMethodWithResolverContext(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IHttpContextAccessor httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();

        return $"Custom method with resolver context so you can resolve the services needed: {httpContextAccessor.HttpContext?.Request.Path}";
    }
}
```

When making your model you can use properties and methods to expose data. When using methods you can add parameters to the method to make it more dynamic. You can also use the `IResolverContext` to resolve services from the DI container. Every parameter except the `IResolverContext` will be a part of the GraphQL schema and will be a paramter you pass to the query.

2. Extend the query where you want the model to be present. In this example, we extend the `ContentByIdQuery` query:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class CustomContentItemExampleQuery : ContentByIdQuery<ContentItem>
{
    [GraphQLName("CustomContentItemExampleQuery")]
    public override ContentItem? ContentById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        return base.ContentById(resolverContext, id, inContext);
    }

    protected override ContentItem? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        return contentItemRepository.GetContentItem(new Nikcio.UHeadless.Defaults.ContentItems.ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            StatusCode = publishedContent == null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
            Redirect = null
        });
    }
}

```

If you don't already have the `ContentByIdQuery` you don't need to override the `ContentById` method as you don't need to alter the `GraphQLName` which is the name of the query in the schema. This defaults to the query names you would normally use when using the query. (In this example `contentById`)

3. Register the query in Nikcio.UHeadless:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<CustomContentItemExampleQuery>();
})
```

4. Open `/graphql` and observe your new model for the `CustomContentItemExampleQuery` query.

## Example extended the content model with public access settings:

1. Create your access rule model:

```csharp
using HotChocolate;

[GraphQLDescription("Represents an access rule for the restrict public access settings.")]
public class AccessRuleModel
{
    public AccessRuleModel(string? ruleType, string? ruleValue)
    {
        RuleType = ruleType ?? string.Empty;
        RuleValue = ruleValue ?? string.Empty;
    }

    [GraphQLDescription("Gets the type of protection to grant access to the content item.")]
    public string RuleType { get; set; }

    [GraphQLDescription("Gets the name of who has access to the content item.")]
    public string RuleValue { get; set; }
}

```

2. Create your Permissions model:

```csharp
using HotChocolate;

[GraphQLDescription("Represents a restrict public access settings of a content item.")]
public class PermissionsModel
{
    public PermissionsModel()
    {
        AccessRules = [];
    }

    [GraphQLDescription("Gets the url to the login page.")]
    public string? UrlLogin { get; set; }

    [GraphQLDescription("Gets the url to the error page.")]
    public string? UrlNoAccess { get; set; }

    [GraphQLDescription("Gets the access rules for the restrict public access settings.")]
    public List<AccessRuleModel> AccessRules { get; set; }
}
```

3. Create your content model:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;

/// <summary>
/// This example demonstrates how to create a custom content item that includes the public access settings from Umbraco.
/// with this information you can block access to content based on the user using the site.
/// </summary>
/// <remarks>
/// This example uses the default <see cref="Nikcio.UHeadless.Defaults.ContentItems.ContentItem"/> class from UHeadless
/// but could also be implemented using the <see cref="Nikcio.UHeadless.ContentItems.ContentItemBase"/> class.
/// </remarks>
[GraphQLName("PublishAccessExampleContentItem")]
public class ContentItem : Nikcio.UHeadless.Defaults.ContentItems.ContentItem
{
    public ContentItem(CreateCommand command) : base(command)
    {
    }

    [GraphQLDescription("Gets the restrict public access settings of the content item.")]
    public PermissionsModel? Permissions(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        ILogger<ContentItem> logger = resolverContext.Service<ILogger<ContentItem>>();
        IContentService contentService = resolverContext.Service<IContentService>();
        IPublicAccessService publicAccessService = resolverContext.Service<IPublicAccessService>();
        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        if (PublishedContent == null)
        {
            logger.LogWarning("Content is null");
            return null;
        }

        IContent? content = contentService.GetById(PublishedContent.Id);

        if (content == null)
        {
            logger.LogWarning("Content from content service is null. Id: {ContentId}", PublishedContent.Id);
            return null;
        }

        PublicAccessEntry? entry = publicAccessService.GetEntryForContent(content);

        if (entry == null)
        {
            logger.LogWarning("Public access entry is null. ContentId: {ContentId}", PublishedContent.Id);
            return null;
        }

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContent? loginContent = contentCache.GetById(entry.LoginNodeId);
        IPublishedContent? noAccessContent = contentCache.GetById(entry.NoAccessNodeId);

        var permissions = new PermissionsModel
        {
            UrlLogin = loginContent?.Url(resolverContext.Service<IPublishedUrlProvider>(), resolverContext.Culture(), UrlMode.Absolute),
            UrlNoAccess = noAccessContent?.Url(resolverContext.Service<IPublishedUrlProvider>(), resolverContext.Culture(), UrlMode.Absolute)
        };

        foreach (PublicAccessRule rule in entry.Rules)
        {
            permissions.AccessRules.Add(new AccessRuleModel(rule.RuleType, rule.RuleValue));
        }

        return permissions;
    }
}
```

4. Extend the query where you want the model to be present. In this example, we extend the `ContentByIdQuery` query:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.ContentItems;
using Umbraco.Cms.Core.Models.PublishedContent;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class PublishAccessExampleQuery : ContentByIdQuery<ContentItem>
{
    [GraphQLName("publishAccessExampleQuery")]
    public override ContentItem? ContentById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id,
        [GraphQLDescription("The context of the request.")] QueryContext? inContext = null)
    {
        return base.ContentById(resolverContext, id, inContext);
    }

    protected override ContentItem? CreateContentItem(IPublishedContent? publishedContent, IContentItemRepository<ContentItem> contentItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(contentItemRepository);

        return contentItemRepository.GetContentItem(new Nikcio.UHeadless.Defaults.ContentItems.ContentItem.CreateCommand()
        {
            PublishedContent = publishedContent,
            ResolverContext = resolverContext,
            StatusCode = publishedContent == null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
            Redirect = null
        });
    }
}

```

3. Register the query in Nikcio.UHeadless:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<PublishAccessExampleQuery>();
})
```

4. Open `/graphql` and observe your new model for the `publishAccessExampleQuery` query.
