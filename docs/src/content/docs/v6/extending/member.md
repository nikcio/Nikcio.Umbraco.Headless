---
title: Extending Member Data Structures in Nikcio.UHeadless
description: Learn how to extend the member model in Nikcio.UHeadless.
---

Nikcio.UHeadless provides flexibility to extend and replace member data structures to accommodate your specific needs. This documentation outlines three examples of how you can extend the member data structures.

## Example custom member model extending the existing member model:

1. Create your own member model by inheriting from `Nikcio.UHeadless.Defaults.Members.MemberItem`:

```csharp
using HotChocolate.Resolvers;
using HotChocolate;

/// <summary>
/// This example demonstrates how to create a custom member item with custom properties and methods.
/// </summary>
/// <remarks>
/// The <see cref="IResolverContext"/> can be used to resolve services from the DI container like you normally would with dependency injection.
/// It's important to contain any logic to the specific property or method within the property or method itself if possiable.
/// As GraphQL will only call the properties or methods that are requestedm and may not call all of them.
/// </remarks>
[GraphQLName("CustomMemberItemExampleMemberItem")]
public class MemberItem : Nikcio.UHeadless.Defaults.Members.MemberItem
{
    public MemberItem(CreateCommand command) : base(command)
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

When making your model you can use properties and methods to expose data. When using methods you can add parameters to the method to make it more dynamic. You can also use the `IResolverContext` to resolve services from the DI container. Every parameter except the `IResolverContext` will be a part of the GraphQL schema and will be a paramter you pass to the query.

2. Extend the query where you want the model to be present. In this example, we extend the `MemberByIdQuery` query:

```csharp
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.MemberItems;
using Umbraco.Cms.Core.Models.PublishedContent;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class CustomMemberItemExampleQuery : MemberByIdQuery<MemberItem>
{
    [GraphQLName("CustomMemberItemExampleQuery")]
    public override MemberItem? MemberById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id)
    {
        return base.MemberById(resolverContext, id);
    }

    protected override MemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<MemberItem> memberItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);

        return memberItemRepository.GetMemberItem(new Nikcio.UHeadless.Members.MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        });
    }
}

```

If you don't already have the `MemberByIdQuery` you don't need to override the `MemberById` method as you don't need to alter the `GraphQLName` which is the name of the query in the schema. This defaults to the query names you would normally use when using the query. (In this example `MemberById`)

3. Register the query in Nikcio.UHeadless:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<CustomMemberItemExampleQuery>();
})
```

4. Open `/graphql` and observe your new model for the `CustomMemberItemExampleQuery` query.
