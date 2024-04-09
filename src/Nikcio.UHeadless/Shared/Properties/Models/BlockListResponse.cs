using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Shared.Properties.Models;

/// <summary>
/// Represents a block list model
/// </summary>
[GraphQLDescription("Represents a block list model.")]
public class BlockListResponse : PropertyValue
{
    private readonly BlockListModel? _blockListModel;

    /// <summary>
    /// Gets the blocks of a block list model
    /// </summary>
    [GraphQLDescription("Gets the blocks of a block list model.")]
    public List<BlockListItemResponse>? Blocks => _blockListModel?.Select(blockListItem =>
    {
        return new BlockListItemResponse(blockListItem, ResolverContext);
    }).ToList();

    public BlockListResponse(CreateCommand command) : base(command)
    {
        _blockListModel = PublishedProperty.Value<BlockListModel>(PublishedValueFallback, Culture, Segment, Fallback);
    }
}


/// <inheritdoc/>
[GraphQLDescription("Represents a block list item.")]
public class BlockListItemResponse
{
    private readonly BlockListItem _blockListItem;
    private readonly IResolverContext _resolverContext;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content properties of the block list item.")]
    public TypedBlockListContentProperties ContentProperties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.BlockListItemContent, _blockListItem);
        _resolverContext.SetScopedState(ContextDataKeys.BlockListItemContentPropertyName, nameof(ContentProperties));
        return new TypedBlockListContentProperties();
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the setting properties of the block list item.")]
    public TypedBlockListSettingsProperties SettingsProperties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.BlockListItemSettings, _blockListItem);
        _resolverContext.SetScopedState(ContextDataKeys.BlockListItemSettingsPropertyName, nameof(SettingsProperties));
        return new TypedBlockListSettingsProperties();
    }

    /// <summary>
    /// Gets the alias of the content block list item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the content block list item.")]
    public string? ContentAlias => _blockListItem.Content?.ContentType?.Alias;

    /// <summary>
    /// Gets the alias of the settings block list item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the settings block list item.")]
    public string? SettingsAlias => _blockListItem.Settings?.ContentType?.Alias;

    public BlockListItemResponse(BlockListItem blockListItem, IResolverContext resolverContext)
    {
        _blockListItem = blockListItem;
        _resolverContext = resolverContext;
    }
}
