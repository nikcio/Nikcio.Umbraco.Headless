using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.BlockList.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a block list item.")]
[Obsolete("Use Defaults.Properties.BlockListItem instead. The main difference here is that the content and setting properties are now typed properties which means you will have to create your GraphQL query so that you select the values that you need.")]
public class BasicBlockListItem : BasicBlockListItem<BasicProperty>
{
    public BasicBlockListItem(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext) : base(blockListItem, resolverContext)
    {
    }
}

/// <inheritdoc/>
[GraphQLDescription("Represents a block list item.")]
[Obsolete("Use Defaults.Properties.BlockListItem instead. The main difference here is that the content and setting properties are now typed properties which means you will have to create your GraphQL query so that you select the values that you need.")]
public class BasicBlockListItem<TProperty> : BlockListItem
    where TProperty : IProperty
{
    public BasicBlockListItem(Umbraco.Cms.Core.Models.Blocks.BlockListItem blockListItem, IResolverContext resolverContext) : base(blockListItem, resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IPropertyFactory<TProperty> propertyFactory = resolverContext.Service<IPropertyFactory<TProperty>>();

        IPublishedContent? publishedContent = resolverContext.GetScopedState<IPublishedContent>(ContextDataKeys.PublishedContent);
        string? culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        string? segment = resolverContext.GetScopedState<string?>(ContextDataKeys.Segment);
        Umbraco.Cms.Core.Models.PublishedContent.Fallback fallback = resolverContext.GetScopedState<Umbraco.Cms.Core.Models.PublishedContent.Fallback>(ContextDataKeys.Fallback);

        if (publishedContent != null)
        {
            foreach (IPublishedProperty property in Item.Content.Properties)
            {
                ContentProperties.Add(propertyFactory.GetProperty(property, publishedContent, culture, segment, fallback));
            }

            if (Item.Settings != null)
            {
                foreach (IPublishedProperty property in Item.Settings.Properties)
                {
                    SettingsProperties.Add(propertyFactory.GetProperty(property, publishedContent, culture, segment, fallback));
                }
            }
        }
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content properties of the block list item.")]
    [Obsolete("Transition to typed properties instead using the Defaults.Properties.BlockListItem model.")]
    public new List<TProperty?> ContentProperties { get; set; } = new();

    /// <inheritdoc/>
    [GraphQLDescription("Gets the setting properties of the block list item.")]
    [Obsolete("Transition to typed properties instead using the Defaults.Properties.BlockListItem model.")]
    public new List<TProperty?> SettingsProperties { get; set; } = new();
}
