using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockGrid.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a block grid area.")]
[Obsolete("Use Defaults.Properties.BlockGridArea instead.")]
public class BasicBlockGridArea : BasicBlockGridArea<BasicBlockGridItem>
{
    public BasicBlockGridArea(Umbraco.Cms.Core.Models.Blocks.BlockGridArea blockGridArea, IResolverContext resolverContext) : base(blockGridArea, resolverContext)
    {
    }

    protected override BasicBlockGridItem CreateBlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext)
    {
        return new BasicBlockGridItem(blockGridItem, resolverContext);
    }
}

/// <inheritdoc/>
[Obsolete("Use Defaults.Properties.BlockGridArea instead.")]
public abstract class BasicBlockGridArea<TBlockGridItem> : BlockGridArea<TBlockGridItem>
    where TBlockGridItem : class
{
    protected BasicBlockGridArea(Umbraco.Cms.Core.Models.Blocks.BlockGridArea blockGridArea, IResolverContext resolverContext) : base(blockGridArea, resolverContext)
    {
    }
}
