using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Maps;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Constants;
using Nikcio.UHeadless.Core.Reflection.Factories;

namespace Nikcio.UHeadless.Base.Properties.Factories;

/// <inheritdoc/>
public class PropertyValueFactory : IPropertyValueFactory
{
    /// <summary>
    /// A map of what class to use for a property
    /// </summary>
    protected IPropertyMap propertyMap { get; }

    /// <summary>
    /// A factory that can create object with DI
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <inheritdoc/>
    public PropertyValueFactory(IPropertyMap propertyMapper, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        propertyMap = propertyMapper;
        this.dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <inheritdoc/>
    public virtual PropertyValue? GetPropertyValue(CreatePropertyValue createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue, nameof(createPropertyValue));

        if (createPropertyValue.Property.PropertyType.ContentType == null)
        {
            return default;
        }

        string propertyTypeName = propertyMap.GetPropertyTypeName(createPropertyValue.Property.PropertyType.ContentType.Alias,
                                                                  createPropertyValue.Property.PropertyType.Alias,
                                                                  createPropertyValue.Property.PropertyType.EditorAlias);
        var type = Type.GetType(propertyTypeName);
        if (type == null)
        {
            return null;
        }

        return dependencyReflectorFactory.GetReflectedType<PropertyValue>(type, new object[] { createPropertyValue });
    }
}
