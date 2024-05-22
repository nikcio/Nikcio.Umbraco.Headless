using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Properties;
using Nikcio.UHeadless.Reflection;

namespace Nikcio.UHeadless.Base.Properties.Factories;

/// <inheritdoc/>
[Obsolete("Convert to using typed properties instead")]
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
    public virtual Models.PropertyValue? GetPropertyValue(CreatePropertyValue createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

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

        return dependencyReflectorFactory.GetReflectedType<Models.PropertyValue>(type, [createPropertyValue]);
    }
}
