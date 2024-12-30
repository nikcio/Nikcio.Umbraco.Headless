# Nikcio.UHeadless

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nikcio_Nikcio.UHeadless&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nikcio_Nikcio.UHeadless)
[![Build UHeadless](https://github.com/nikcio/Nikcio.UHeadless/actions/workflows/build.yml/badge.svg)](https://github.com/nikcio/Nikcio.UHeadless/actions/workflows/build.yml)
![Nuget Downloads](https://img.shields.io/nuget/dt/Nikcio.UHeadless?color=%230078d7&label=Nuget%20downloads&logo=Nuget)
![Nuget Version](https://img.shields.io/nuget/v/Nikcio.UHeadless?label=Stable%20version)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Nikcio.UHeadless?label=Prerelease%20version)

Welcome to Nikcio.UHeadless, a powerful package that enables you to create a headless GraphQL interface for your Umbraco CMS. This package provides an easy to setup solution for exposing your data and offers a wide range of extensibility options to tailor the headless functionality to your specific needs.

## Compatibility

The Nikcio.UHeadless package is compatible with the following Umbraco versions:

| Umbraco version      | Supported Version     |
|----------------------|-----------------------|
| Umbraco 10           | v3.x.x                |
| Umbraco 12           | v4.x.x                |
| Umbraco 13           | v4.2.x+ & v5.x.x      |
| Umbraco 14           | v6.x.x                |
| Umbraco 15           | v7.x.x                |

For more information, please refer to the [Versioning](#versioning) section.

## Setup

### Installation

To install the Nikcio.UHeadless package, run the following command:

```shell
dotnet add Nikcio.UHeadless
```

You can also find the package on [NuGet](https://www.nuget.org/packages/Nikcio.UHeadless).

### Integration

To integrate the package into your project, follow these steps:

1. Open your `Program.cs` file.
2. Add the following using statements:

    ```csharp
    using Nikcio.UHeadless;
    using Nikcio.UHeadless.Defaults.ContentItems;
    ```

3. On the `UmbracoBuilder`, add the following code:

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

4. Then after the `app.BootUmbracoAsync()` method, add the following code:

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

With these configurations in place, your content will be available at `/graphql`. 

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

## Documentation

For detailed documentation and usage instructions, please refer to the [Nikcio.UHeadless Documentation](https://nikcio.github.io/Nikcio.UHeadless).

## Versioning

Nikcio.UHeadless follows the principles of Semantic Versioning to ensure consistency. The version numbers have the following meaning:

```
vX.Y.Z
```

- X (Major): Indicates a breaking change.
- Y (Minor): Signifies a feature change.
- Z (Patch): Represents bug fixes.

### Full version table

| Umbraco version      | Supported Versions    | Development                           |
|----------------------|-----------------------|---------------------------------------|
| Umbraco 9            | v1.x.x & v2.x.x       | No development                        |
| Umbraco 10           | v2.x.x & v3.x.x       | Only reported issues for v3.x.x       |
| Umbraco 11           | v3.x.x & v4.x.x       | No development                        |
| Umbraco 12           | v4.x.x                | Only reported issues for v4.x.x       |
| Umbraco 13           | v4.2.x+ & v5.x.x      | Only reported issues for v5.x.x       |
| Umbraco 14           | v6.x.x                | No development                        |
| Umbraco 15           | v7.x.x                | Active branch                         |

## Contributing

We welcome contributions to Nikcio.UHeadless. Please refer to the [Contributing Guide](CONTRIBUTING.md) for more information on how to get involved.

---

**Sponsor Nikcio.UHeadless Development**

If you find Nikcio.UHeadless valuable and would like to support its ongoing development, consider sponsoring the project through [GitHub Sponsors](https://github.com/sponsors/nikcio/). Your sponsorship helps ensure the continued improvement and maintenance of this package. Thank you for your support!
