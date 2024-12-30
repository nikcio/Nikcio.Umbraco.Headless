using HotChocolate;
using HotChocolate.Resolvers;

namespace Code.Examples.Headless.CustomMediaItemExample;

/// <summary>
/// This example demonstrates how to create a custom media item with custom properties and methods.
/// </summary>
/// <remarks>
/// The <see cref="IResolverContext"/> can be used to resolve services from the DI container like you normally would with dependency injection.
/// It's important to contain any logic to the specific property or method within the property or method itself if possiable.
/// As GraphQL will only call the properties or methods that are requestedm and may not call all of them.
/// </remarks>
[GraphQLName("CustomMediaItemExampleMediaItem")]
public class MediaItem : Nikcio.UHeadless.Defaults.MediaItems.MediaItem
{
    public MediaItem(CreateCommand command) : base(command)
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
