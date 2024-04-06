using Nikcio.UHeadless.Base.Elements.Commands;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Elements.Models;

/// <inheritdoc/>
public abstract class Element<TProperty> : IElement
    where TProperty : IProperty
{
    /// <inheritdoc/>
    protected Element(CreateElement createElement, IPropertyFactory<TProperty> propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(createElement);

        Content = createElement.Content;
        Culture = createElement.Culture;
        PropertyFactory = propertyFactory;
        Segment = createElement.Segment;
        Fallback = createElement.Fallback;
    }

    /// <summary>
    /// The propertyFactory
    /// </summary>
    [GraphQLIgnore]
    public virtual IPropertyFactory<TProperty> PropertyFactory { get; }

    /// <summary>
    /// The content
    /// </summary>
    [GraphQLIgnore]
    public virtual IPublishedContent? Content { get; }

    /// <summary>
    /// The culture
    /// </summary>
    [GraphQLIgnore]
    public virtual string? Culture { get; }

    /// <summary>
    /// The segment
    /// </summary>
    [GraphQLIgnore]
    public virtual string? Segment { get; }

    /// <summary>
    /// The property fallback
    /// </summary>
    [GraphQLIgnore]
    public virtual Fallback? Fallback { get; }
}
