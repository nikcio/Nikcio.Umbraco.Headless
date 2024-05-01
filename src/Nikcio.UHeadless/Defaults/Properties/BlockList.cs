using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a block list model
/// </summary>
[GraphQLDescription("Represents a block list model.")]
public class BlockList : BlockList<BlockListItem>
{
    public BlockList(CreateCommand command) : base(command)
    {
    }

    protected override BlockListItem CreateBlock(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext)
    {
        return new BlockListItem(blockListItem, resolverContext);
    }
}

/// <summary>
/// Represents a block list model
/// </summary>
[GraphQLDescription("Represents a block list model.")]
public abstract class BlockList<TBlockListItem> : PropertyValue
    where TBlockListItem : class
{
    /// <summary>
    /// The block list model
    /// </summary>
    protected BlockListModel? BlockListModel { get; }

    /// <summary>
    /// Gets the blocks of a block list model
    /// </summary>
    [GraphQLDescription("Gets the blocks of a block list model.")]
    public List<TBlockListItem>? Blocks(IResolverContext resolverContext)
    {
        return BlockListModel?.Select(blockListItem =>
        {
            return CreateBlock(blockListItem, resolverContext);
        }).ToList();
    }

    protected BlockList(CreateCommand command) : base(command)
    {
        IResolverContext resolverContext = command.ResolverContext;
        BlockListModel = PublishedProperty.Value<BlockListModel>(PublishedValueFallback, resolverContext.Culture(), resolverContext.Segment(), resolverContext.Fallback());
    }

    /// <summary>
    /// Creates a block
    /// </summary>
    /// <param name="blockListItem"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TBlockListItem CreateBlock(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext);
}


/// <inheritdoc/>
[GraphQLDescription("Represents a block list item.")]
public class BlockListItem
{
    /// <summary>
    /// The block list item
    /// </summary>
    /// <value></value>
    protected Umbraco.Cms.Core.Models.Blocks.BlockListItem Item { get; }

    /// <summary>
    /// The resolver context
    /// </summary> <summary>
    /// 
    /// </summary>
    /// <value></value>
    protected IResolverContext ResolverContext { get; }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content properties of the block list item.")]
    public TypedBlockListContentProperties ContentProperties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.BlockListItemContent, Item);
        ResolverContext.SetScopedState(ContextDataKeys.BlockListItemContentPropertyName, nameof(ContentProperties));
        return new TypedBlockListContentProperties();
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the setting properties of the block list item.")]
    public TypedBlockListSettingsProperties SettingsProperties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.BlockListItemSettings, Item);
        ResolverContext.SetScopedState(ContextDataKeys.BlockListItemSettingsPropertyName, nameof(SettingsProperties));
        return new TypedBlockListSettingsProperties();
    }

    /// <summary>
    /// Gets the alias of the content block list item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the content block list item.")]
    public string? ContentAlias => Item.Content?.ContentType?.Alias;

    /// <summary>
    /// Gets the alias of the settings block list item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the settings block list item.")]
    public string? SettingsAlias => Item.Settings?.ContentType?.Alias;

    public BlockListItem(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext)
    {
        Item = blockListItem;
        ResolverContext = resolverContext;
    }
}
