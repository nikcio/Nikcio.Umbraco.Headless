---
title: Extending the Block list model in Nikcio.UHeadless
description: Learn how to extend the block list model in Nikcio.UHeadless.
---

Nikcio.UHeadless provides flexibility to extend and replace the block list model to accommodate your specific needs.

## Example

1. Create your block list model:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;

namespace Code.Examples.Headless.CustomBlockListExample;

[GraphQLName("CustomBlockList")]
public class BlockList : Nikcio.UHeadless.Defaults.Properties.BlockList<BlockListItem>
{
    public BlockList(CreateCommand command) : base(command)
    {
    }

    protected override BlockListItem CreateBlock(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext)
    {
        return new BlockListItem(blockListItem, resolverContext);
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

public class BlockListItem : Nikcio.UHeadless.Defaults.Properties.BlockListItem
{
    public BlockListItem(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext) : base(blockListItem, resolverContext)
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

2. Register the model in the UHeadless options:

```csharp
.AddUHeadless(options =>
{
    options.AddEditorMapping<BlockList>(Constants.PropertyEditors.Aliases.BlockList);
})
```

There are two methods available to register the model in the UHeadless options:

| Method Name       | Description                                                                    |
|-------------------|--------------------------------------------------------------------------------|
| AddAliasMapping   | Adds a mapping of a type to a content type alias and property type alias.      |
| AddEditorMapping  | Adds a mapping of a type to an editor alias.                                   |


## Resolver context extensions

The `IResolverContext` contains the following extensions that can be used to get information about the current query:

| Extension Name       | Description                                                                                                                      |
|----------------------|----------------------------------------------------------------------------------------------------------------------------------|
| IncludePreview()     | Gets whether to include preview content in the query.                                                                            |
| Culture()            | Gets the culture of the query.                                                                                                   |
| Segment()            | Gets the segment of the query.                                                                                                   |
| Fallback()           | Gets the fallback of the query to be used for property values.                                                                   |
| PublishedContent()   |Gets the published content from the scoped data. This can be different depending on where in the query resolution the code runs.  |
