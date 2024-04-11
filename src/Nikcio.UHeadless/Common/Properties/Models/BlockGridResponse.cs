using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents a block grid property value
/// </summary>
[GraphQLDescription("Represents a block grid property value.")]
public class BlockGridResponse : PropertyValue
{
    private readonly BlockGridModel? _propertyValue;

    /// <summary>
    /// Gets the blocks of a block grid model
    /// </summary>
    [GraphQLDescription("Gets the blocks of a block grid model.")]
    public List<BlockGridItemResponse>? Blocks => _propertyValue?.Select(blockGridItem =>
    {
        return new BlockGridItemResponse(blockGridItem, ResolverContext);
    }).ToList();

    /// <summary>
    /// Gets the number of columns defined for the grid
    /// </summary>
    [GraphQLDescription("Gets the number of columns defined for the grid.")]
    public int? GridColumns => _propertyValue?.GridColumns;

    public BlockGridResponse(CreateCommand command) : base(command)
    {
        _propertyValue = PublishedProperty.Value<BlockGridModel>(PublishedValueFallback, Culture, Segment, Fallback);
    }
}

/// <summary>
/// Represents a block grid item
/// </summary>
[GraphQLDescription("Represents a block grid item.")]
public class BlockGridItemResponse
{
    private readonly BlockGridItem _blockGridItem;
    private readonly IResolverContext _resolverContext;

    /// <summary>
    /// Gets the content properties of the block grid item
    /// </summary>
    [GraphQLDescription("Gets the content properties of the block grid item.")]
    public TypedBlockGridContentProperties ContentProperties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.BlockGridItemContent, _blockGridItem);
        _resolverContext.SetScopedState(ContextDataKeys.BlockGridItemContentPropertyName, nameof(ContentProperties));
        return new TypedBlockGridContentProperties();
    }

    /// <summary>
    /// Gets the setting properties of the block grid item
    /// </summary>
    [GraphQLDescription("Gets the setting properties of the block grid item.")]
    public TypedBlockGridSettingsProperties SettingsProperties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.BlockGridItemSettings, _blockGridItem);
        _resolverContext.SetScopedState(ContextDataKeys.BlockGridItemSettingsPropertyName, nameof(SettingsProperties));
        return new TypedBlockGridSettingsProperties();
    }

    /// <summary>
    /// Gets the alias of the content block grid item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the content block grid item.")]
    public string? ContentAlias => _blockGridItem.Content?.ContentType?.Alias;

    /// <summary>
    /// Gets the alias of the settings block grid item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the settings block grid item.")]
    public string? SettingsAlias => _blockGridItem.Settings?.ContentType?.Alias;

    /// <summary>
    /// Gets the areas of the block grid item.
    /// </summary>
    [GraphQLDescription("Gets the areas of the block grid item.")]
    public List<BlockGridAreaResponse> Areas => _blockGridItem.Areas.Select(blockGridArea =>
    {
        return new BlockGridAreaResponse(blockGridArea, _resolverContext);
    }).ToList();

    /// <summary>
    /// Gets the row dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the row dimensions of the block.")]
    public int RowSpan => _blockGridItem.RowSpan;

    /// <summary>
    /// Gets the column dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the column dimensions of the block.")]
    public int ColumnSpan => _blockGridItem.ColumnSpan;

    public BlockGridItemResponse(BlockGridItem blockGridItem, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(blockGridItem);

        _blockGridItem = blockGridItem;
        _resolverContext = resolverContext;

    }

    public class CreateCommand
    {
        /// <summary>
        /// The block grid item
        /// </summary>
        public required BlockGridItem BlockGridItem { get; init; }
    }
}

/// <summary>
/// Represents a block grid area
/// </summary>
[GraphQLDescription("Represents a block grid area.")]
public class BlockGridAreaResponse
{
    private readonly BlockGridArea _blockGridArea;
    private readonly IResolverContext _resolverContext;

    /// <summary>
    /// Gets the blocks of the block grid area
    /// </summary>
    [GraphQLDescription("Gets the blocks of the block grid area.")]
    public List<BlockGridItemResponse>? Blocks => _blockGridArea.Select(blockGridItem =>
    {
        return new BlockGridItemResponse(blockGridItem, _resolverContext);
    }).ToList();

    /// <summary>
    /// Gets the alias of the block grid area.
    /// </summary>
    [GraphQLDescription("Gets the alias of the block grid area.")]
    public string AreaAlias => _blockGridArea.Alias;

    /// <summary>
    /// Gets the row dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the row dimensions of the block.")]
    public int RowSpan => _blockGridArea.RowSpan;

    /// <summary>
    /// Gets the column dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the column dimensions of the block.")]
    public int ColumnSpan => _blockGridArea.ColumnSpan;

    public BlockGridAreaResponse(BlockGridArea blockGridArea, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(blockGridArea);

        _blockGridArea = blockGridArea;
        _resolverContext = resolverContext;
    }
}
