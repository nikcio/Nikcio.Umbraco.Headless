---
title: Extending Media Data Structures in Nikcio.UHeadless
description: Learn how to extend the media model in Nikcio.UHeadless.
---

Nikcio.UHeadless provides flexibility to extend and replace media data structures to accommodate your specific needs. This documentation outlines three examples of how you can extend the media data structures.

## Example custom media model extending the existing media model:

1. Create your own media model by inheriting from `Nikcio.UHeadless.Defaults.MediaItems.MediaItem`:

```csharp
using HotChocolate.Resolvers;
using HotChocolate;

/// <summary>
/// This example demonstrates how to create a custom media item with custom properties and methods.
/// </summary>
/// <remarks>
/// The <see cref="IResolverContext"/> can be used to resolve services from the DI container like you normally would with dependency injection.
/// It's important to contain any logic to the specific property or method within the property or method itself if possiable.
/// As GraphQL will only call the properties or methods that are requestedm and may not call all of them.
/// </remarks>
[GraphQLName("CustomMediaItemExampleMediaItem")]
public class MediaItem : Nikcio.UHeadless.Defaults.MediaItems.MediaItem
{
    public MediaItem(CreateCommand command) : base(command)
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

2. Extend the query where you want the model to be present. In this example, we extend the `MediaByIdQuery` query:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.MediaItems;
using Umbraco.Cms.Core.Models.PublishedContent;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class CustomMediaItemExampleQuery : MediaByIdQuery<MediaItem>
{
    [GraphQLName("CustomMediaItemExampleQuery")]
    public override MediaItem? MediaById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id)
    {
        return base.MediaById(resolverContext, id);
    }

    protected override MediaItem? CreateMediaItem(IPublishedContent? media, IMediaItemRepository<MediaItem> mediaItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(mediaItemRepository);

        return mediaItemRepository.GetMediaItem(new Nikcio.UHeadless.Media.MediaItemBase.CreateCommand()
        {
            PublishedContent = media,
            ResolverContext = resolverContext,
        });
    }
}
```

If you don't already have the `MediaByIdQuery` you don't need to override the `MediaById` method as you don't need to alter the `GraphQLName` which is the name of the query in the schema. This defaults to the query names you would normally use when using the query. (In this example `MediaById`)

3. Register the query in Nikcio.UHeadless:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<CustomMediaItemExampleQuery>();
})
```

4. Open `/graphql` and observe your new model for the `CustomMediaItemExampleQuery` query.
