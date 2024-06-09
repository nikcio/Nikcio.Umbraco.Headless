---
title: Extending the Media Picker model in Nikcio.UHeadless
description: Learn how to extend the media picker model in Nikcio.UHeadless.
---

Nikcio.UHeadless provides flexibility to extend and replace the media picker model to accommodate your specific needs.

## Example 1

1. Create your own media picker model:

```csharp
using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Code.Examples.Headless.CustomMediaPickerExample;

public class MediaPicker : Nikcio.UHeadless.Defaults.Properties.MediaPicker<MediaPickerItem>
{
    public MediaPicker(CreateCommand command) : base(command)
    {
    }

    protected override MediaPickerItem CreateMediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new MediaPickerItem(publishedContent, resolverContext);
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

public class MediaPickerItem : Nikcio.UHeadless.Defaults.Properties.MediaPickerItem
{
    public MediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext) : base(publishedContent, resolverContext)
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
    options.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MediaPicker);
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
