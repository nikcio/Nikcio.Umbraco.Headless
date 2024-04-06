using Nikcio.UHeadless.Base.Properties.EditorsValues.BlockGrid.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.BlockGrid.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockGrid.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a block grid area.")]
public class BasicBlockGridArea : BasicBlockGridArea<BasicBlockGridItem>
{
    /// <inheritdoc/>
    public BasicBlockGridArea(CreateBlockGridArea createBlockGridArea, IDependencyReflectorFactory dependencyReflectorFactory) : base(createBlockGridArea, dependencyReflectorFactory)
    {
    }
}

/// <inheritdoc/>
public class BasicBlockGridArea<TBlockGridItem> : BlockGridArea
    where TBlockGridItem : BlockGridItem
{
    /// <inheritdoc/>
    public BasicBlockGridArea(CreateBlockGridArea createBlockGridArea, IDependencyReflectorFactory dependencyReflectorFactory) : base(createBlockGridArea)
    {
        ArgumentNullException.ThrowIfNull(createBlockGridArea);

        Alias = createBlockGridArea.BlockGridArea.Alias;
        RowSpan = createBlockGridArea.BlockGridArea.RowSpan;
        ColumnSpan = createBlockGridArea.BlockGridArea.ColumnSpan;

        Blocks = createBlockGridArea.BlockGridArea?.Select(blockGridItem =>
        {
            Type type = typeof(TBlockGridItem);
            return dependencyReflectorFactory.GetReflectedType<TBlockGridItem>(type, new object[] { new CreateBlockGridItem(createBlockGridArea.Content, blockGridItem, createBlockGridArea.Culture, createBlockGridArea.Segment, createBlockGridArea.Fallback) });
        }).OfType<TBlockGridItem>().ToList();
    }

    /// <summary>
    /// Gets the blocks of the block grid area
    /// </summary>
    [GraphQLDescription("Gets the blocks of the block grid area.")]
    public virtual List<TBlockGridItem>? Blocks { get; set; }

    /// <summary>
    /// Gets the alias of the block grid area.
    /// </summary>
    [GraphQLDescription("Gets the alias block grid area.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Alias is a name native to the Umbraco landscape and should therefore be easy to understand.")]
    public virtual string? Alias { get; set; }

    /// <summary>
    /// Gets the row dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the row dimensions of the block.")]
    public virtual int RowSpan { get; set; }

    /// <summary>
    /// Gets the column dimensions of the block.
    /// </summary>
    [GraphQLDescription("Gets the column dimensions of the block.")]
    public virtual int ColumnSpan { get; set; }
}
