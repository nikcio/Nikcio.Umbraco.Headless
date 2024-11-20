---
title: Content Queries
description: Learn how to query content in Nikcio.UHeadless.
---

The Nikcio.UHeadless package provides various content queries that allow you to retrieve content items in different ways from Umbraco CMS.

## Queries

You can add any query to the UHeadless options as seen here:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<ContentByRouteQuery>();
})
```

The following content queries are available:

| Query class Name             | Description                                 | Needed claim values                                 |
|------------------------------|---------------------------------------------|-----------------------------------------------------|
| ContentAtRootQuery           | Gets all the content items at root level.   | content.at.root.query or global.content.read        |
| ContentByContentTypeQuery    | Gets all the content items by content type. | content.by.contentType.query or global.content.read |
| ContentByGuidQuery           | Gets a content item by Guid.                | content.by.guid.query or global.content.read        |
| ContentByIdQuery             | Gets a content item by id.                  | content.by.id.query or global.content.read          |
| ContentByRouteQuery          | Gets a content item by a route.             | content.by.route.query or global.content.read       |
| ContentByTagQuery            | Gets content items by tag.                  | content.by.tag.query or global.content.read         |

You can explore these queries and their parameters in the UI provided at `/graphql` when you have added them to the UHeadless options as seen above.

The claim values are needed when having authorization enabled. You can read more about authorization and how to create tokens in the [Security Considerations](../security) section.

A special case for claim values are for the member picker editor. To access the data of the member picker you will need one of the following claim values: `property.values.member.picker` or `global.member.read`.

## Next steps

- [Building your property query](./properties)