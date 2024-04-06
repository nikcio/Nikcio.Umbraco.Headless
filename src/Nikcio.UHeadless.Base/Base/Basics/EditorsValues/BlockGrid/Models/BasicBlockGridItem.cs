using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.EditorsValues.BlockGrid.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.BlockGrid.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockGrid.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a block grid item.")]
public class BasicBlockGridItem : BasicBlockGridItem<BasicProperty>
{
    /// <inheritdoc/>
    public BasicBlockGridItem(CreateBlockGridItem createBlockGridItem, IPropertyFactory<BasicProperty> propertyFactory, IDependencyReflectorFactory dependencyReflectorFactory) : base(createBlockGridItem, propertyFactory, dependencyReflectorFactory)
    {
    }
}

/// <inheritdoc/>
[GraphQLDescription("Represents a block grid item.")]
public class BasicBlockGridItem<TProperty> : BasicBlockGridItem<TProperty, BasicBlockGridArea>
    where TProperty : IProperty
{
    /// <inheritdoc/>
    public BasicBlockGridItem(CreateBlockGridItem createBlockGridItem, IPropertyFactory<TProperty> propertyFactory, IDependencyReflectorFactory dependencyReflectorFactory) : base(createBlockGridItem, propertyFactory, dependencyReflectorFactory)
    {
    }
}

/// <inheritdoc/>
[GraphQLDescription("Represents a block Grid item.")]
public class BasicBlockGridItem<TProperty, TBlockGridArea> : BlockGridItem
    where TProperty : IProperty
    where TBlockGridArea : BlockGridArea
{
    /// <inheritdoc/>
    public BasicBlockGridItem(CreateBlockGridItem createBlockGridItem, IPropertyFactory<TProperty> propertyFactory, IDependencyReflectorFactory dependencyReflectorFactory) : base(createBlockGridItem)
    {
        ArgumentNullException.ThrowIfNull(createBlockGridItem);
        ArgumentNullException.ThrowIfNull(propertyFactory);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        if (createBlockGridItem.BlockGridItem == null)
        {
            return;
        }
        if (createBlockGridItem.Content != null)
        {
            ContentAlias = createBlockGridItem.BlockGridItem.Content.ContentType?.Alias;
            foreach (Umbraco.Cms.Core.Models.PublishedContent.IPublishedProperty property in createBlockGridItem.BlockGridItem.Content.Properties)
            {
                ContentProperties.Add(propertyFactory.GetProperty(property, createBlockGridItem.Content, createBlockGridItem.Culture, createBlockGridItem.Segment, createBlockGridItem.Fallback));
            }

            if (createBlockGridItem.BlockGridItem.Settings != null)
            {
                SettingsAlias = createBlockGridItem.BlockGridItem.Settings.ContentType?.Alias;
                foreach (Umbraco.Cms.Core.Models.PublishedContent.IPublishedProperty property in createBlockGridItem.BlockGridItem.Settings.Properties)
                {
                    SettingsProperties.Add(propertyFactory.GetProperty(property, createBlockGridItem.Content, createBlockGridItem.Culture, createBlockGridItem.Segment, createBlockGridItem.Fallback));
                }
            }

            if (createBlockGridItem.BlockGridItem.Areas != null)
            {
                foreach (Umbraco.Cms.Core.Models.Blocks.BlockGridArea area in createBlockGridItem.BlockGridItem.Areas)
                {
                    var createBlockGridArea = new CreateBlockGridArea(createBlockGridItem.Content, area, createBlockGridItem.Culture, createBlockGridItem.Segment, createBlockGridItem.Fallback);
                    Areas.Add(dependencyReflectorFactory.GetReflectedType<TBlockGridArea>(typeof(TBlockGridArea), new object[] { createBlockGridArea }));
                }
            }

            RowSpan = createBlockGridItem.BlockGridItem.RowSpan;

            ColumnSpan = createBlockGridItem.BlockGridItem.ColumnSpan;
        }
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content properties of the block grid item.")]
    public virtual List<TProperty?> ContentProperties { get; set; } = new();

    /// <inheritdoc/>
    [GraphQLDescription("Gets the setting properties of the block grid item.")]
    public virtual List<TProperty?> SettingsProperties { get; set; } = new();

    /// <summary>
    /// Gets the alias of the content block grid item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the content block grid item.")]
    public virtual string? ContentAlias { get; set; }

    /// <summary>
    /// Gets the alias of the settings block grid item.
    /// </summary>
    [GraphQLDescription("Gets the alias of the settings block grid item.")]
    public virtual string? SettingsAlias { get; set; }

    /// <summary>
    /// Gets the areas of the block grid item.
    /// </summary>
    [GraphQLDescription("Gets the areas of the block grid item.")]
    public virtual List<TBlockGridArea?> Areas { get; set; } = new();

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
