using HotChocolate.Resolvers;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockGrid.Models;

/// <summary>
/// Represents a block grid model
/// </summary>
[GraphQLDescription("Represents a block list model.")]
[Obsolete("Use Defaults.Properties.BlockGrid instead.")]
public class BasicBlockGridModel : BasicBlockGridModel<BasicBlockGridItem>
{
    public BasicBlockGridModel(CreateCommand command) : base(command)
    {
    }

    protected override BasicBlockGridItem CreateBlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext)
    {
        return new BasicBlockGridItem(blockGridItem, resolverContext);
    }
}

/// <summary>
/// Represents a block grid model
/// </summary>
/// <typeparam name="TBlockGridItem"></typeparam>
[GraphQLDescription("Represents a block list model.")]
[Obsolete("Use Defaults.Properties.BlockGrid instead.")]
public abstract class BasicBlockGridModel<TBlockGridItem> : Defaults.Properties.BlockGrid<TBlockGridItem>
    where TBlockGridItem : class
{
    protected BasicBlockGridModel(CreateCommand command) : base(command)
    {
    }
}
