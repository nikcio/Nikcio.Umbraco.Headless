using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a block grid property value
/// </summary>
[GraphQLDescription("Represents a block grid property value.")]
public class BlockGrid : BlockGrid<BlockGridItem>
{
    public BlockGrid(CreateCommand command) : base(command)
    {
    }

    protected override BlockGridItem CreateBlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext)
    {
        return new BlockGridItem(blockGridItem, resolverContext);
    }
}

/// <summary>
/// Represents a block grid property value
/// </summary>
[GraphQLDescription("Represents a block grid property value.")]
public abstract class BlockGrid<TBlockGridItem> : PropertyValue
    where TBlockGridItem : class
{
    /// <summary>
    /// The value of the property
    /// </summary>
    protected BlockGridModel? PropertyValue { get; }

    /// <summary>
    /// Gets the blocks of a block grid model
    /// </summary>
    [GraphQLDescription("Gets the blocks of a block grid model.")]
    public List<TBlockGridItem>? Blocks(IResolverContext resolverContext)
    {
        return PropertyValue?.Select(blockGridItem =>
        {
            return CreateBlockGridItem(blockGridItem, resolverContext);
        }).ToList();
    }

    /// <summary>
    /// Gets the number of columns defined for the grid
    /// </summary>
    [GraphQLDescription("Gets the number of columns defined for the grid.")]
    public int? GridColumns => PropertyValue?.GridColumns;

    protected BlockGrid(CreateCommand command) : base(command)
    {
        IResolverContext resolverContext = command.ResolverContext;
        PropertyValue = PublishedProperty.Value<BlockGridModel>(PublishedValueFallback, resolverContext.Culture(), resolverContext.Segment(), resolverContext.Fallback());
    }

    /// <summary>
    /// Creates a block grid item
    /// </summary>
    /// <param name="blockGridItem"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TBlockGridItem CreateBlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext);
}

/// <summary>
/// Represents a block grid item
/// </summary>
[GraphQLDescription("Represents a block grid item.")]
public class BlockGridItem : BlockGridItem<BlockGridArea>
{
    public BlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext) : base(blockGridItem, resolverContext)
    {
    }

    protected override BlockGridArea CreateBlockGridArea(Umbraco.Cms.Core.Models.Blocks.BlockGridArea blockGridArea, IResolverContext resolverContext)
    {
        return new BlockGridArea(blockGridArea, resolverContext);
    }
}

/// <summary>
/// Represents a block grid item
/// </summary>
[GraphQLDescription("Represents a block grid item.")]
public abstract class BlockGridItem<TBlockGridArea>
    where TBlockGridArea : class
{
    /// <summary>
    /// The block grid item
    /// </summary>
    protected Umbraco.Cms.Core.Models.Blocks.BlockGridItem Item { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    protected IResolverContext ResolverContext { get; }

    /// <summary>
    /// Gets the content properties of the block grid item
    /// </summary>
    [GraphQLDescription("Gets the content properties of the block grid item.")]
    public TypedBlockGridContentProperties ContentProperties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.BlockGridItemContent, Item);
        ResolverContext.SetScopedState(ContextDataKeys.BlockGridItemContentPropertyName, nameof(ContentProperties));
        return new TypedBlockGridContentProperties();
    }

    /// <summary>
    /// Gets the setting properties of the block grid item
    /// </summary>
    [GraphQLDescription("Gets the setting properties of the block grid item.")]
    public TypedBlockGridSettingsProperties SettingsProperties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.BlockGridItemSettings, Item);
        ResolverContext.SetScopedState(ContextDataKeys.BlockGridItemSettingsPropertyName, nameof(SettingsProperties));
        return new TypedBlockGridSettingsProperties();
    }

    /// <summary>
    /// Gets the alias of the content block grid item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the content block grid item.")]
    public string? ContentAlias => Item.Content?.ContentType?.Alias;

    /// <summary>
    /// Gets the alias of the settings block grid item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the settings block grid item.")]
    public string? SettingsAlias => Item.Settings?.ContentType?.Alias;

    /// <summary>
    /// Gets the areas of the block grid item.
    /// </summary>
    [GraphQLDescription("Gets the areas of the block grid item.")]
    public List<TBlockGridArea> Areas()
    {
        return Item.Areas.Select(blockGridArea =>
        {
            return CreateBlockGridArea(blockGridArea, ResolverContext);
        }).ToList();
    }

    /// <summary>
    /// Gets the row dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the row dimensions of the block.")]
    public int RowSpan => Item.RowSpan;

    /// <summary>
    /// Gets the column dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the column dimensions of the block.")]
    public int ColumnSpan => Item.ColumnSpan;

    protected BlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(blockGridItem);

        Item = blockGridItem;
        ResolverContext = resolverContext;
    }

    /// <summary>
    /// Creates a block grid area
    /// </summary>
    /// <param name="blockGridArea"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TBlockGridArea CreateBlockGridArea(Umbraco.Cms.Core.Models.Blocks.BlockGridArea blockGridArea, IResolverContext resolverContext);
}

/// <summary>
/// Represents a block grid area
/// </summary>
[GraphQLDescription("Represents a block grid area.")]
public class BlockGridArea : BlockGridArea<BlockGridItem>
{
    public BlockGridArea(Umbraco.Cms.Core.Models.Blocks.BlockGridArea blockGridArea, IResolverContext resolverContext) : base(blockGridArea, resolverContext)
    {
    }

    protected override BlockGridItem CreateBlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext)
    {
        return new BlockGridItem(blockGridItem, resolverContext);
    }
}

/// <summary>
/// Represents a block grid area
/// </summary>
[GraphQLDescription("Represents a block grid area.")]
public abstract class BlockGridArea<TBlockGridItem>
    where TBlockGridItem : class
{
    /// <summary>
    /// The block grid area
    /// </summary>
    protected Umbraco.Cms.Core.Models.Blocks.BlockGridArea Area { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    protected IResolverContext ResolverContext { get; }

    /// <summary>
    /// Gets the blocks of the block grid area
    /// </summary>
    [GraphQLDescription("Gets the blocks of the block grid area.")]
    public List<BlockGridItem>? Blocks()
    {
        return Area.Select(blockGridItem =>
        {
            return new BlockGridItem(blockGridItem, ResolverContext);
        }).ToList();
    }

    /// <summary>
    /// Gets the alias of the block grid area.
    /// </summary>
    [GraphQLDescription("Gets the alias of the block grid area.")]
    public string Alias => Area.Alias;

    /// <summary>
    /// Gets the row dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the row dimensions of the block.")]
    public int RowSpan => Area.RowSpan;

    /// <summary>
    /// Gets the column dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the column dimensions of the block.")]
    public int ColumnSpan => Area.ColumnSpan;

    protected BlockGridArea(Umbraco.Cms.Core.Models.Blocks.BlockGridArea blockGridArea, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(blockGridArea);

        Area = blockGridArea;
        ResolverContext = resolverContext;
    }

    /// <summary>
    /// Creates a block grid item
    /// </summary>
    /// <param name="blockGridItem"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TBlockGridItem CreateBlockGridItem(Umbraco.Cms.Core.Models.Blocks.BlockGridItem blockGridItem, IResolverContext resolverContext);
}
