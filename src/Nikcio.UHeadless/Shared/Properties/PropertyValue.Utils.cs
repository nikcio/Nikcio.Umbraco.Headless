using Nikcio.UHeadless.Shared.Reflection;

namespace Nikcio.UHeadless.Shared.Properties;

public partial class PropertyValue
{
    public static PropertyValue? CreatePropertyValue(CreateCommand command, IPropertyMap propertyMap, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(propertyMap);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        if (command.PublishedProperty.PropertyType.ContentType == null)
        {
            throw new InvalidOperationException("The content type of the property type is null");
        }

        string propertyTypeName = propertyMap.GetPropertyTypeName(command.PublishedProperty.PropertyType.ContentType.Alias,
                                                                  command.PublishedProperty.PropertyType.Alias,
                                                                  command.PublishedProperty.PropertyType.EditorAlias);

        var type = Type.GetType(propertyTypeName);

        if (type == null)
        {
            throw new InvalidOperationException($"Could not find the type {propertyTypeName}");
        }

        return CreatePropertyValue(command, type, dependencyReflectorFactory);
    }

    public static TPropertyValue? CreatePropertyValue<TPropertyValue>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TPropertyValue : PropertyValue
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        return (TPropertyValue?) CreatePropertyValue(command, typeof(TPropertyValue), dependencyReflectorFactory);
    }

    private static PropertyValue? CreatePropertyValue(CreateCommand command, Type type, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        PropertyValue? createdPropertyValue = dependencyReflectorFactory.GetReflectedType<PropertyValue>(type, new object[] { command });

        if (createdPropertyValue == null)
        {
            return default;
        }
        else
        {
            return createdPropertyValue;
        }
    }
}
