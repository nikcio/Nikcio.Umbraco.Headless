---
title: Endpoint Options
description: Overview of the endpoint options in Nikcio.UHeadless.
---

The UHeadless endpoint can be customized using the builder returned from the `.MapUHeadless()` method. The builder provides a number of options to configure the endpoint.

## Disabling the GraphQL IDE in production

```csharp
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
```

## GraphQLServerOptions Properties

| Property                                | Description                                                                                        |
|-----------------------------------------|----------------------------------------------------------------------------------------------------|
| Tool                                    | The options for configuring the GraphQL tool (e.g., Banana Cake Pop).                              |
| Sockets                                 | The options for configuring GraphQL sockets.                                                       |
| AllowedGetOperations                    | Specifies which GraphQL options are allowed on GET requests.                                       |
| EnableGetRequests                       | Specifies whether GraphQL HTTP GET requests are allowed.                                           |
| EnforceGetRequestsPreflightHeader       | Specifies whether to enforce the preflight header for GraphQL HTTP GET requests.                   |
| EnableMultipartRequests                 | Specifies whether GraphQL HTTP multipart requests are allowed.                                     |
| EnforceMultipartRequestsPreflightHeader | Specifies whether to enforce the preflight header for GraphQL HTTP multipart requests.             |
| EnableSchemaRequests                    | Specifies whether the GraphQL schema SDL can be downloaded.                                        |
| EnableBatching                          | Specifies whether request batching is enabled.                                                     |