using HotChocolate.Resolvers;

namespace Code.Examples.Headless.CustomBlockListExample;

[GraphQLName("CustomBlockList")]
public class BlockList : Nikcio.UHeadless.Defaults.Properties.BlockList<BlockListItem>
{
    public BlockList(CreateCommand command) : base(command)
    {
    }

    protected override BlockListItem CreateBlock(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext)
    {
        return new BlockListItem(blockListItem, resolverContext);
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

public class BlockListItem : Nikcio.UHeadless.Defaults.Properties.BlockListItem
{
    public BlockListItem(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext) : base(blockListItem, resolverContext)
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
