using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Properties.Factories;

/// <inheritdoc/>
[Obsolete("Convert to using typed properties instead")]
public class PropertyFactory<TProperty> : IPropertyFactory<TProperty>
    where TProperty : IProperty
{
    /// <summary>
    /// A factory for creating objects with DI
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <summary>
    /// The published value fallback
    /// </summary>
    protected IPublishedValueFallback publishedValueFallback { get; }

    /// <inheritdoc/>
    public PropertyFactory(IDependencyReflectorFactory dependencyReflectorFactory, IPublishedValueFallback publishedValueFallback)
    {
        this.dependencyReflectorFactory = dependencyReflectorFactory;
        this.publishedValueFallback = publishedValueFallback;
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Property is the name given to this data by Umbraco and it would be confusing to change.")]
    public virtual TProperty? GetProperty(IPublishedProperty property, IPublishedContent publishedContent, string? culture, string? segment, Fallback? fallback)
    {
        var createPropertyCommand = new CreateProperty(property, culture, publishedContent, segment, publishedValueFallback, fallback);

        IProperty? createdProperty = dependencyReflectorFactory.GetReflectedType<IProperty>(typeof(TProperty), [createPropertyCommand]);
        return createdProperty == null ? default : (TProperty) createdProperty;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TProperty?> CreateProperties(IPublishedContent publishedContent, string? culture, string? segment, Fallback? fallback)
    {
        ArgumentNullException.ThrowIfNull(publishedContent);

        return publishedContent.Properties.Select(IPublishedProperty => GetProperty(IPublishedProperty, publishedContent, culture, segment, fallback));
    }
}
