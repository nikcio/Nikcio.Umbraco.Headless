---
title: Adding your custom editor model in Nikcio.UHeadless
description: Learn how to add your custom editor model in Nikcio.UHeadless.
---

Nikcio.UHeadless provides flexibility to add your models for custom property editors.

## Example

1. Create your custom editor model:

```csharp
using HotChocolate.Resolvers;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Common.Properties;

public class CustomEditor : PropertyValue
{
    public CustomEditor(CreateCommand command) : base(command)
    {
        ArgumentNullException.ThrowIfNull(command);

        IResolverContext resolverContext = command.ResolverContext;
        Value = PublishedProperty.Value<string>(PublishedValueFallback, resolverContext.Culture(), resolverContext.Segment(), resolverContext.Fallback());
    }

    public string? Value { get; }
}
```

2. Register the model in the UHeadless options:

```csharp
.AddUHeadless(options =>
{
    options.AddEditorMapping<CustomEditor>("customEditorAlias");
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
