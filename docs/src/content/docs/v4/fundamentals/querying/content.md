---
title: Content Queries
description: Learn how to query content in Nikcio.UHeadless.
---

The Nikcio.UHeadless package provides various content queries that allow you to retrieve content items in different ways from Umbraco CMS. These queries are divided into two variations: "Basic" and "Auth" queries.

## Basic Queries

The "Basic" queries do not require authorization and provide unrestricted access to CMS data. Use the following code example to add a basic content query to the UHeadless options:

```csharp
.AddUHeadless(new()
{
    UHeadlessGraphQLOptions = new()
    {
        GraphQLExtensions = (IRequestExecutorBuilder builder) =>
        {
            builder.UseContentQueries(); // Use this from v4.1.0+ (Only add one)
            builder.AddTypeExtension<BasicContentAllQuery>();
            return builder;
        },
    },
})
```

**Do note that adding the code above will override the defaults and remove the `contentAtRoot` query. To use the `contentAtRoot` query, you need to add the `BasicContentAtRootQuery` to the options.**

The following basic content queries are available:

| Query class Name                            | Description                                                         |
|---------------------------------------------|---------------------------------------------------------------------|
| BasicContentAllQuery                        | Gets all the content items available.                               |
| BasicContentAtRootQuery                     | Gets all the content items at the root level.                       |
| BasicContentByAbsoluteRouteQuery            | Gets a content item by an absolute route.                           |
| BasicContentByContentTypeQuery              | Gets all the content items by content type.                         |
| BasicContentByGuidQuery                     | Gets a content item by GUID.                                        |
| BasicContentByIdQuery                       | Gets a content item by ID.                                          |
| BasicContentByTagQuery                      | Gets content items by tag.                                          |
| BasicContentDescendantsByAbsoluteRouteQuery | Gets content item descendants by an absolute route.                 |
| BasicContentDescendantsByContentTypeQuery   | Gets all descendants of content items with a specific content type. |
| BasicContentDescendantsByGuidQuery          | Gets descendants on a content item selected by GUID.                |
| BasicContentDescendantsByIdQuery            | Gets descendants on a content item selected by ID.                  |

You can explore these queries and their parameters in the UI provided at `/graphql` when you have added them to the `UHeadlessGraphQLOptions.GraphQLExtensions` like in the example above.

## Auth Queries

The "Auth" queries require authentication when querying data. "Auth" queries are "Basic" queries that have been overridden and added the `[Authorize]` attribute from `using HotChocolate.Authorization`. Use the following code example to add an authenticated content query to the UHeadless options:

```csharp
.AddUHeadless(new()
{
    UHeadlessGraphQLOptions = new()
    {
        GraphQLExtensions = (IRequestExecutorBuilder builder) =>
        {
            builder.UseContentQueries(); // Use this from v4.1.0+ (Only add one)
            builder.AddTypeExtension<AuthContentAllQuery>();
            return builder;
        },
    },
})
```

**Do note that adding the code above will override the defaults and remove the `contentAtRoot` query. To use the `contentAtRoot` query, you need to add the `BasicContentAtRootQuery` to the options.**

The following authenticated content queries are available:

| Query class Name                            | Description                                                         |
|---------------------------------------------|---------------------------------------------------------------------|
| AuthContentAllQuery                         | Gets all the content items available.                               |
| AuthContentAtRootQuery                      | Gets all the content items at the root level.                       |
| AuthContentByAbsoluteRouteQuery             | Gets a content item by an absolute route.                           |
| AuthContentByContentTypeQuery               | Gets all the content items by content type.                         |
| AuthContentByGuidQuery                      | Gets a content item by GUID.                                        |
| AuthContentByIdQuery                        | Gets a content item by ID.                                          |
| AuthContentByTagQuery                       | Gets content items by tag.                                          |
| AuthContentDescendantsByAbsoluteRouteQuery  | Gets content item descendants by an absolute route.                 |
| AuthContentDescendantsByContentTypeQuery    | Gets all descendants of content items with a specific content type. |
| AuthContentDescendantsByGuidQuery           | Gets descendants on a content item selected by GUID.                |
| AuthContentDescendantsByIdQuery             | Gets descendants on a content item selected by ID.                  |

You can explore these queries and their parameters in the UI provided at `/graphql` when you have added them to the `UHeadlessGraphQLOptions.GraphQLExtensions` like in the example above.

## Queries

_Explore the most up to date information when the query is registered in your application like in the example in the sections above. Then the query information will be available at `/graphql`_

### ContentAll

Gets all the content items available.

Parameters:

- **culture**: The culture.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentAtRoot

Gets all the content items at the root level.

Parameters:

- **culture**: The culture.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentByAbsoluteRoute

Gets a content item by an absolute route.

Parameters:

- **route**: The route to fetch. Example: '/da/frontpage/'.
- **baseUrl**: The base URL for the request. Example: 'https://my-website.com/'. Default is the current domain.
- **culture**: The culture.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **routeMode**: Modes for requesting by route.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentByContentType

Gets all the content items by content type.

Parameters:

- **contentType**: The contentType to fetch.
- **culture**: The culture.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentByGuid

Gets a content item by GUID.

Parameters:

- **id**: The ID to fetch.
- **culture**: The culture to fetch.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentById

Gets a content item by ID.

Parameters:

- **id**: The ID to fetch.
- **culture**: The culture to fetch.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentByTag

Gets content items by tag.

Parameters:

- **tag**: The tag to fetch.
- **tagGroup**: The tag group to fetch.
- **culture**: The culture to fetch.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentDescendantsByAbsoluteRoute

Gets content item descendants by an absolute route.

Parameters:

- **route**: The route to fetch. Example: '/da/frontpage/'.
- **baseUrl**: The base URL for the request. Example: 'https://my-website.com/'. Default is the current domain.
- **culture**: The culture.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **routeMode**: Modes for requesting by

 route.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentDescendantsByContentType

Gets all descendants of content items with a specific content type.

Parameters:

- **contentType**: The contentType to fetch.
- **culture**: The culture.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentDescendantsByGuid

Gets descendants of a content item selected by GUID.

Parameters:

- **id**: The ID to fetch.
- **culture**: The culture to fetch.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

### ContentDescendantsById

Gets descendants of a content item selected by ID.

Parameters:

- **id**: The ID to fetch.
- **culture**: The culture to fetch.
- **preview**: Fetch preview values. Preview will show unpublished items.
- **segment**: The property variation segment.
- **fallback**: The property value fallback strategy.

## Next steps

When creating your GraphQL queries for media, the properties section can be a little difficult to wrap your head around. Therefore, you can find some documentation about how you can query this here.

- [Building your property query](./properties)