using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.EditorsValues.NestedContent.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.NestedContent.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models;

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
public class BasicNestedContentElement : BasicNestedContentElement<BasicProperty>
{
    /// <inheritdoc/>
    public BasicNestedContentElement(CreateNestedContentElement createElement, IPropertyFactory<BasicProperty> propertyFactory) : base(createElement, propertyFactory)
    {
    }
}

/// <summary>
/// Represents nested content
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[GraphQLDescription("Represents nested content.")]
public class BasicNestedContentElement<TProperty> : NestedContentElement
    where TProperty : IProperty
{
    /// <summary>
    /// Gets the properties of the nested content
    /// </summary>
    [GraphQLDescription("Gets the properties of the nested content.")]
    public virtual List<TProperty?> Properties { get; set; } = new();

    /// <inheritdoc/>
    public BasicNestedContentElement(CreateNestedContentElement createElement, IPropertyFactory<TProperty> propertyFactory) : base(createElement)
    {
        ArgumentNullException.ThrowIfNull(createElement);
        ArgumentNullException.ThrowIfNull(propertyFactory);

        if (createElement.Element != null)
        {
            foreach (Umbraco.Cms.Core.Models.PublishedContent.IPublishedProperty property in createElement.Element.Properties)
            {
                Properties.Add(propertyFactory.GetProperty(property, createElement.Content, createElement.Culture, createElement.Segment, createElement.Fallback));
            }
        }
    }
}
