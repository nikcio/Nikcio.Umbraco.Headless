---
title: Building Your Property Query
description: Learn how to build your property query in Nikcio.UHeadless.
---

In GraphQL, queries are used to request specific data from a server. When building a property query in GraphQL, you can use fragments to organize and reuse common selections of fields. Fragments allow you to define a set of fields that can be included in multiple queries, reducing duplication and making queries more modular.

## Quering properties

To query properties you use a list of property aliases that you want to fetch. This is possible on content, media and member models. To query properties you use the content, content compositions, media or member types created in Umbraco. Example:

```graphql
query {
  contentByRoute(baseUrl: "", route: "/") {
    properties {
      ... on IArticle {
        title {
          value
        }
        articleDate {
          value
        }
        author {
          items {
            name
          }
        }
      }
      __typename
    }
  }
}
```

This works by the properties getting the type of content your fetching and then fetching the properties based on that type. In this example we have created a content type called `Article` in Umbraco and if the content item we find with our query matches the `Article` type we fetch the properties `title`, `articleDate` and `author`. 

The `__typename` field is used to get the type of the content item we fetch. This can help you figure out what properties are available on the content item you fetch. In this example if the `__typename` field returns `Article` we know that the properties `title`, `articleDate` and `author` are available on the content item.

This is because all types of content, media and member types which have properties have a matching type and interface in the GraphQL schema.

For this example we will see the following in the GraphQL schema:

```graphql
type Article implements IArticleControls & IContentControls & IHeaderControls & IMainImageControls & ISEOControls & IVisibilityControls & IArticle {

# ... the fields of the type ...

}
```

Here we can see that the `Article` type implements the `IArticle` interface. This means that we can fetch the properties of the `IArticle` interface on the `Article` type. This is why we can fetch the properties `title`, `articleDate` and `author` on the `Article` type.

Other than is we can see multiple other interfaces that the `Article` type implements. This means that we can fetch the properties of those interfaces on the `Article` type as well. This is useful when you have compositions in Umbraco that you want to fetch properties from.

For example we can see that the `IArticleControls` interface has the following properties:

```graphql
type ArticleControls implements IArticleControls {
  """
  Enter the date for the article
  """
  articleDate: DateTimePicker

  author: ContentPicker

  categories: ContentPicker
}
```

This means that we can use a fragment selection on the `IArticleControls` interface to fetch the properties `articleDate`, `author` and `categories` on the any type that has this compostion in Umbraco.

:::note
When fetching properties you need to use the interfaces (They are prefixed with `I` on the type name) instead of the concrete types as the concrete types don't always return the expected values due to the matching rules in GraphQL.
:::