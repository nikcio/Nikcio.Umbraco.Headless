---
title: Security Considerations
description: Learn about what areas to consider when securing your Nikcio.UHeadless application.
---

When using Nikcio.UHeadless, it is important to consider security measures to protect your GraphQL data and ensure that access to sensitive information is properly controlled. This section highlights some key security considerations and provides guidance on implementing security measures.

## Using Authentication and Authorization in Nikcio.UHeadless

To enable authentication and authorization in Nikcio.UHeadless follow the code example below:

### Step 1: Configure the Services in the `Program.cs` file

In the `AddUHeadless` options add the following code snippet:

:::note
The `.AddAuth()` call must be the first thing in the options
:::

```csharp
.AddUHeadless(options =>
{
    options.AddAuth(new()
    {
        ApiKey = builder.Configuration.GetValue<string>("UHeadless:ApiKey") ?? throw new InvalidOperationException("No value for UHeadless:ApiKey was found"),
        Secret = builder.Configuration.GetValue<string>("UHeadless:Secret") ?? throw new InvalidOperationException("No value for UHeadless:Secret was found"),
    });

    // Other configuration
})
```

The `ApiKey` can be any value over 32 characters long. This value is used in the `X-UHeadless-Api-Key` header when creating tokens used for quering the GraphQL endpoint. 

The `Secret` can be any value over 64 characters long. This value is used as the signing key when creating the JWT tokens used for quering the GraphQL endpoint.

:::note
You can provide the `ApiKey` and `Secret` from anywhere this example shows loading them from the `appsettings.json` file.
:::

Also make sure to add the following code snippet to the request pipeline after the `app.BootUmbracoAsync()` method:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Step 2: Create a token for queries to the GraphQL endpoint

Once the configuration above have been added all the default queries in the package will automatically be protected and require a token to be queried. To create a token run the application, go to `/graphql` and run the following query:

```graphql
mutation {
  createToken(claims: [
    {
      name: "headless-scope",
      value: "global.content.read"
    }
  ]) {
    expires
    header
    prefix
    token
  }
}
```

Make sure to include the `X-UHeadless-Api-Key` header with the value of the `ApiKey` you provided in the configuration. This will create a token that can query all content queries.

![Create token example image](../../../../assets/v5/create-token-example-image.png)

### Step 3: Use the token to query the GraphQL endpoint

The `createToken` mutation will return a token in the following format:

```json
{
  "data": {
    "createToken": {
      "expires": 1717883090,
      "header": "X-UHeadless-Token",
      "prefix": "Bearer ",
      "token": "<TOKEN>"
    }
  }
}
```

To query a query use the token provided along with the prefix in the header `X-UHeadless-Token` like so:

```graphql
query {
  contentByRoute(baseUrl: "", route: "/") {
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

```text
X-UHeadless-Token: Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJoZWFkbGVzcy1zY29wZSI6Imdsb2JhbC5jb250ZW50LnJlYWQiLCJleHAiOjE3MTc3OTIyMTYsImlzcyI6Ik5pa2Npby5VSGVhZGxlc3MiLCJhdWQiOiJOaWtjaW8uVUhlYWRsZXNzIn0.Iwis2EPUjk4uY9_ZyDLYg3NRs2lTh8S-7KEaVCQTFv6nKywMmZRiPMN4Fe2uanq44WqmzWeHRqOWhp5tCGHfKA
```

![Use token example image](../../../../assets/v5/use-token-example-image.png)

Now you have successfully added authentication and authorization to your Nikcio.UHeadless application.

## Different claims for different queries

To create a token with different claims for different queries you can use the `createToken` mutation like so:

```graphql
mutation {
  createToken(claims: [
    {
      name: "headless-scope",
      value: ["global.content.read", "global.media.read"]
    }
  ]) {
    expires
    header
    prefix
    token
  }
}
```

or

```graphql
mutation {
  createToken(claims: [
    {
      name: "headless-scope",
      value: "global.content.read"
    },
    {
      name: "headless-scope",
      value: "global.media.read"
    }
  ]) {
    expires
    header
    prefix
    token
  }
}
```

There exists a few different claims in the default queries for UHeadless. What claims are required for each query can be found in the documentation for the query.

See [content queries](../querying/content), [media queries](../querying/media) or [member queries](../querying/members) for more information.

A special case for claim values are for the member picker editor. To access the data of the member picker you will need one of the following claim values: `property.values.member.picker` or `global.member.read`.

## Adding security to your own queries

If you have created your own queries you can add security to them by adding the policy configuration in the `ApplyConfiguration` method and adding the `[Authorize]` attribute from `HotChocolate.Authorization` to the query method like so:

:::caution
It's important to use the `[Authorize]` from `HotChocolate.Authorization` and not the one from `Microsoft.AspNetCore.Authorization` as the one from `HotChocolate.Authorization` is the one that works with HotChocolate and adding the other one will have no effect.
:::

```csharp
using HotChocolate.Authorization;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Authorization;

public class MyCustomQuery : IGraphQLQuery
{
    public const string PolicyName = "MyCustomQuery";

    public const string ClaimValue = "my.custom.query";

    [GraphQLIgnore]
    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorizationBuilder().AddPolicy(PolicyName, policy =>
        {
            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadless);

            policy.RequireAuthenticatedUser();

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue);
        });
    }

    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("My custom query.")]
    public virtual List<string> MyQuery()
    {
        return ["Hello", "World"];
    }
}
```

## Utillty queries

There is one utility query that can be used to find all default claims are used by the queries you have added to your application. This query is called `utility_GetClaimGroups` and can be used like so:

:::note
The `utility_GetClaimGroups` query is publicly accessible and should only be used for development purposes.
:::

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<UtilityClaimGroupsQuery>();
})
```

Query:

```graphql
query {
  utility_GetClaimGroups {
    groupName
    claimValues {
      name
      values
    }
  }
}
```

Response example:

```json
{
  "data": {
    "utility_GetClaimGroups": [
      {
        "groupName": "Members",
        "claimValues": [
          {
            "name": "headless-scope",
            "values": [
              "property.values.member.picker",
              "global.member.read",
              "find.members.by.display.name.query",
              "find.members.by.email.query",
              "find.members.by.role.query",
              "find.members.by.username.query",
              "member.by.email.query",
              "member.by.guid.query",
              "member.by.id.query",
              "member.by.username.query"
            ]
          }
        ]
      },
      {
        "groupName": "Content",
        "claimValues": [
          {
            "name": "headless-scope",
            "values": [
              "content.by.route.query",
              "global.content.read",
              "content.by.contentType.query",
              "content.at.root.query",
              "content.by.id.query",
              "content.by.guid.query",
              "content.by.tag.query"
            ]
          }
        ]
      },
      {
        "groupName": "Media",
        "claimValues": [
          {
            "name": "headless-scope",
            "values": [
              "media.by.contentType.query",
              "global.media.read",
              "media.at.root.query",
              "media.by.id.query",
              "media.by.guid.query"
            ]
          }
        ]
      }
    ]
  }
}
```

## HotChocolate Documentation

Nikcio.UHeadless leverages the [HotChocolate](https://chillicream.com/docs/hotchocolate) NuGet package, which is a powerful GraphQL server implementation for .NET. HotChocolate provides documentation on various security topics, including how to add authentication and authorization to your GraphQL queries and data.

For detailed insights into how the implementation works with HotChocolate, refer to the official HotChocolate documentation on security at:

[HotChocolate Security Documentation](https://chillicream.com/docs/hotchocolate/v13/security)

This documentation covers different authentication and authorization mechanisms supported by HotChocolate and provides guidelines on securing your GraphQL API effectively.

## Additional or Alternative Security Measures

In addition to leveraging built-in security features and following best practices for authentication and authorization, you can implement additional security measures to protect your Nikcio.UHeadless GraphQL data. Here are some suggestions:

One suggestion is to use a reverse proxy or similar technology to route all traffic to and from the `/graphql` endpoint internally. By configuring the reverse proxy to allow access only from trusted sources, you can ensure that your GraphQL data remains inaccessible to the public.

Another suggestion is to only route GraphQL traffic server side to your FE application as this will prevent leaking access tokens to the client.