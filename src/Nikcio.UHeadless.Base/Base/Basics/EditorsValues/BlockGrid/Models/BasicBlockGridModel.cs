using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.BlockGrid.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.BlockGrid.Models;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockGrid.Models;

/// <summary>
/// Represents a block grid model
/// </summary>
[GraphQLDescription("Represents a block list model.")]
public class BasicBlockGridModel : BasicBlockGridModel<BasicBlockGridItem>
{
    /// <inheritdoc/>
    public BasicBlockGridModel(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
    }
}

/// <summary>
/// Represents a block grid model
/// </summary>
/// <typeparam name="TBlockGridItem"></typeparam>
[GraphQLDescription("Represents a block list model.")]
public class BasicBlockGridModel<TBlockGridItem> : PropertyValue
    where TBlockGridItem : BlockGridItem
{
    /// <summary>
    /// Gets the blocks of a block grid model
    /// </summary>
    [GraphQLDescription("Gets the blocks of a block grid model.")]
    public virtual List<TBlockGridItem>? Blocks { get; set; }

    /// <summary>
    /// Gets the number of columns defined for the grid
    /// </summary>
    [GraphQLDescription("Gets the number of columns defined for the grid.")]
    public virtual int? GridColumns { get; set; }

    /// <inheritdoc/>
    public BasicBlockGridModel(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        Umbraco.Cms.Core.Models.Blocks.BlockGridModel? propertyValue = createPropertyValue.Property.Value<Umbraco.Cms.Core.Models.Blocks.BlockGridModel>(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);

        Blocks = propertyValue?.Select(blockGridItem =>
        {
            Type type = typeof(TBlockGridItem);
            return dependencyReflectorFactory.GetReflectedType<TBlockGridItem>(type, new object[] { new CreateBlockGridItem(createPropertyValue.Content, blockGridItem, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback) });
        }).OfType<TBlockGridItem>().ToList();

        GridColumns = propertyValue?.GridColumns;
    }
}
