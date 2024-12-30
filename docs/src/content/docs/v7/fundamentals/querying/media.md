---
title: Media Queries
description: Learn how to query media in Nikcio.UHeadless.
---

The Nikcio.UHeadless package provides various media queries that allow you to retrieve media items in different ways from Umbraco CMS.

## Queries

You can add any query to the UHeadless options as seen here:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<MediaByGuidQuery>();
})
```

The following content queries are available:

| Query class Name             | Description                                 | Needed claim values                                 |
|------------------------------|---------------------------------------------|-----------------------------------------------------|
| MediaAtRootQuery             | Gets all the media items at root level.     | media.at.root.query or global.media.read            |
| MediaByContentTypeQuery      | Gets all the media items by content type.   | media.by.contentType.query or global.media.read     |
| MediaByGuidQuery             | Gets a Media item by Guid.                  | media.by.guid.query or global.media.read            |
| MediaByIdQuery               | Gets a Media item by id.                    | media.by.id.query or global.media.read              |

You can explore these queries and their parameters in the UI provided at `/graphql` when you have added them to the UHeadless options as seen above.

The claim values are needed when having authorization enabled. You can read more about authorization and how to create tokens in the [Security Considerations](../security) section.

A special case for claim values are for the member picker editor. To access the data of the member picker you will need one of the following claim values: `property.values.member.picker` or `global.member.read`.

## Next steps

- [Building your property query](./properties)