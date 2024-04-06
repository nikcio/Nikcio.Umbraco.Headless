using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.NestedContent.Commands;
using Nikcio.UHeadless.Base.Properties.EditorsValues.NestedContent.Models;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models;

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
public class BasicNestedContent : BasicNestedContent<BasicNestedContentElement>
{
    /// <inheritdoc/>
    public BasicNestedContent(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
    }
}

/// <summary>
/// Represents nested content
/// </summary>
/// <typeparam name="TNestedContentElement"></typeparam>
[GraphQLDescription("Represents nested content.")]
public class BasicNestedContent<TNestedContentElement> : PropertyValue
    where TNestedContentElement : NestedContentElement
{
    /// <summary>
    /// Gets the elements of a nested content
    /// </summary>
    [GraphQLDescription("Gets the elements of a nested content.")]
    public virtual List<TNestedContentElement>? Elements { get; set; }

    /// <inheritdoc/>
    public BasicNestedContent(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue)
    {
        IEnumerable<IPublishedElement>? propertyValue = createPropertyValue.Property.Value<IEnumerable<IPublishedElement>?>(createPropertyValue.PublishedValueFallback, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback);
        if (propertyValue == null)
        {
            return;
        }

        Elements = propertyValue.Select(element =>
        {
            Type type = typeof(TNestedContentElement);
            return dependencyReflectorFactory.GetReflectedType<TNestedContentElement>(type, new object[] { new CreateNestedContentElement(createPropertyValue.Content, element, createPropertyValue.Culture, createPropertyValue.Segment, createPropertyValue.Fallback) });
        }).OfType<TNestedContentElement>().ToList();
    }
}
