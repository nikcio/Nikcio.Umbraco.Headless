---
title: Getting Started with Nikcio.UHeadless
description: Learn how to get started with Nikcio.UHeadless.
---

This guide will walk you through the process of integrating Nikcio.UHeadless into your Umbraco solution. By following these steps, you'll be able to create a headless GraphQL interface for your Umbraco CMS.

## Installation

To get started, follow the steps below:

### Step 1: Install the package

Install the Nikcio.UHeadless package using the following command:

```shell
dotnet add Nikcio.UHeadless
```

### Step 2: Add the extensions to the `Program.cs` file

In your `Program.cs` file, add the necessary extensions by following the code snippet below:

Namespaces:

```csharp
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.ContentItems;
```

On the `UmbracoBuilder`, add the following code:

```csharp
builder.CreateUmbracoBuilder()
    // Default Umbraco configuration
    .AddUHeadless(options =>
    {
        options.DisableAuthorization = true; // Change this later when adding authentication - See documentation

        options.AddDefaults();

        options.AddQuery<ContentByRouteQuery>();
        options.AddQuery<ContentByGuidQuery>();
    })
    .Build();
```

Then after the `app.BootUmbracoAsync()` method, add the following code:

```csharp
await app.BootUmbracoAsync();

app.UseAuthentication();
app.UseAuthorization();

GraphQLEndpointConventionBuilder graphQLEndpointBuilder = app.MapUHeadless();

// Only enable the GraphQL IDE in development
if (!builder.Environment.IsDevelopment())
{
    graphQLEndpointBuilder.WithOptions(new GraphQLServerOptions()
    {
        Tool =
        {
            Enable = false,
        }
    });
}

app.UseUmbraco()
    // Default Umbraco configuration
```

The `.AddUHeadless()` method adds the services needed for Nikcio.UHeadless to run, while `app.MapUHeadless()` adds the endpoint from where you interact with the GraphQL interface. These extensions provide a range of options that can be customized. To learn more about available options, refer to the [UHeadless options](../reference/options) and [UHeadless endpoint options](../reference/endpoint-options) documentation.

### Step 3: Find the GraphQL endpoint

By default, the GraphQL endpoint can be accessed at `/graphql` in your application.

### Step 4: Make your first query

To get started, try querying your content using their GUIDs or routes. For example with the query below:

__Tip: GUIDs can be found in the info tab when viewing content in the backoffice__

```graphql
query {
  contentByGuid(id: "dcf18a51-6919-4cf8-89d1-36b94ce4d963") {
    id
    key
    name
    statusCode
    templateId
    updateDate
    url(urlMode: ABSOLUTE)
    urlSegment
  }
}
```

This query fetches a content item with the given GUID.

Congratulations! You have successfully integrated Nikcio.UHeadless into your Umbraco solution. 

## Adding a more queries

To add a more queries to Nikcio.UHeadless, you can include the following code in your `Program.cs` file:

```csharp
.AddUHeadless(options =>
{
    // Existing configuration

    options.AddQuery<ContentByIdQuery>();
})
```

This example demonstrates how to add the `ContentByIdQuery` query to Nikcio.UHeadless. By including this code, you will enable the ability to query a content item by the numeric ID instead of the GUID:

```graphql
query {
  contentById(id: 1050) {
    id
    key
    name
    statusCode
    templateId
    updateDate
    url(urlMode: ABSOLUTE)
    urlSegment
  }
}
```

To explore the available queries and how to use them, refer to the following documentation:

- [Learn how to query properties](../querying/properties)
- [Querying Content](../querying/content)
- [Querying Media](../querying/media)
- [Querying Members](../querying/members)

## Next steps

- [Security Considerations](../security)

If you have any questions or need further assistance, don't hesitate to reach out to us. Happy coding with Nikcio.UHeadless