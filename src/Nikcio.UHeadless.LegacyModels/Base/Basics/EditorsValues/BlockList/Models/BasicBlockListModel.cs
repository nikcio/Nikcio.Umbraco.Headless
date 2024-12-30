using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockList.Models;

/// <summary>
/// Represents a block list model
/// </summary>
[GraphQLDescription("Represents a block list model.")]
[Obsolete("Use Defaults.Properties.BlockList instead.")]
public class BasicBlockListModel : BasicBlockListModel<BasicBlockListItem>
{
    public BasicBlockListModel(CreateCommand command) : base(command)
    {
    }

    protected override BasicBlockListItem CreateBlock(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext)
    {
        return new BasicBlockListItem(blockListItem, resolverContext);
    }
}

/// <summary>
/// Represents a block list model
/// </summary>
/// <typeparam name="TBlockListItem"></typeparam>
[GraphQLDescription("Represents a block list model.")]
[Obsolete("Use Defaults.Properties.BlockList instead.")]
public abstract class BasicBlockListModel<TBlockListItem> : BlockList<TBlockListItem>
    where TBlockListItem : class
{
    protected BasicBlockListModel(CreateCommand command) : base(command)
    {
    }
}
