using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Base.Properties.Extensions.Options;

namespace Nikcio.UHeadless.Base.Properties.Extensions;

/// <summary>
/// Map extensions
/// </summary>
public static class MapExtensions
{
    /// <summary>
    /// Adds the property maps
    /// </summary>
    /// <param name="services"></param>
    /// <param name="propertyMapOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddPropertyMaps(this IServiceCollection services, PropertyMapOptions propertyMapOptions)
    {
        ArgumentNullException.ThrowIfNull(propertyMapOptions, nameof(propertyMapOptions));

        services
            .AddSingleton(propertyMapOptions.PropertyMap);

        if (propertyMapOptions.PropertyMappings != null)
        {
            foreach (Action<Maps.IPropertyMap> propertyMapping in propertyMapOptions.PropertyMappings)
            {
                propertyMapping.Invoke(propertyMapOptions.PropertyMap);
            }
        }

        return services;
    }
}
