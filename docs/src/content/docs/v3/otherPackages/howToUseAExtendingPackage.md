---
title: How to use an extending package
description: Learn how to use an extending package in Nikcio.UHeadless.
---

## Install the package from Nuget
First step is to install the package from Nuget. This can be done by:
```
dotnet add NAME_OF_PACKAGE
```

## Register the queries, mutations or subscriptions
If the extending package includes new queries you need to register these in the `startup.cs`. This can be done like this:

```csharp
public void ConfigureServices(IServiceCollection services) {
services.AddUmbraco(_env, _config)
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .AddUHeadless(new() {
        UHeadlessGraphQLOptions = new() {
            GraphQLExtensions = (IRequestExecutorBuilder builder) => {
                builder.AddMaxExecutionDepthRule(10);
                builder.AddTypeExtension<BasicContentQuery>();
                builder.AddTypeExtension<BasicPropertyQuery>();
                builder.AddTypeExtension<BasicMediaQuery>();
                return builder;
            },
        },
    })
    .Build();
}
```

It's important here to also include the queries you're already using because this setting overrules the default settings. By default `BasicContentQuery`, `BasicPropertyQuery` and `BasicMediaQuery` is registered as in the code example above.

## Package queries/mutations/subscriptions

Find what queries/mutations/subscriptions class are available to register in the extending packages here:

### Nikcio.UHeadless.Members

* `BasicMemberQuery`
