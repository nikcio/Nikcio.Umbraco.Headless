---
title: Nikcio.UHeadless Extension Documentation
description: Get an overview of the Nikcio.UHeadless extension documentation.
---

Welcome to the Nikcio.UHeadless Extension Documentation! Here, you'll find resources to help you extend and customize various features of Nikcio.UHeadless. Please select the topic you want to explore:

- [Block List](./block-list): Learn how to extend and replace the block list property with your custom implementation.
- [Custom Editor](./custom-editor): Discover how to create and integrate your custom editor for property values.
- [Media Picker](./media-picker): Find out how to extend and customize the media picker data.
- [Rich Text](./rich-text): Learn how to extend and replace the rich text editor with your custom implementation.

## Dependency Injection

Dependency injection is supported via the `IResolverContext` which is available on the command in the contructor of the property model. This allows you to resolve services and other dependencies needed for your custom implementation.

You can also inject the `IResolverContext` into methods on the property model to resolve services and other dependencies needed for your custom implementation.

### Examples

```csharp
public CustomEditor(CreateCommand command) : base(command)
{
    IHttpContextAccessor httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();
}
```

```csharp
public string? CustomMethodWithResolverContext(IResolverContext resolverContext)
{
    ArgumentNullException.ThrowIfNull(resolverContext);

    IHttpContextAccessor httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();

    return $"Custom method with resolver context so you can resolve the services needed: {httpContextAccessor.HttpContext?.Request.Path}";
}
```

## Resolver context extensions

The `IResolverContext` contains the following extensions that can be used to get information about the current query:

| Extension Name       | Description                                                                                                                      |
|----------------------|----------------------------------------------------------------------------------------------------------------------------------|
| IncludePreview()     | Gets whether to include preview content in the query.                                                                            |
| Culture()            | Gets the culture of the query.                                                                                                   |
| Segment()            | Gets the segment of the query.                                                                                                   |
| Fallback()           | Gets the fallback of the query to be used for property values.                                                                   |
| PublishedContent()   |Gets the published content from the scoped data. This can be different depending on where in the query resolution the code runs.  |
