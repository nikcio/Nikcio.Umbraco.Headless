using Microsoft.Extensions.DependencyInjection;

namespace Nikcio.UHeadless.Base.Properties.Extensions;

/// <summary>
/// Property extensions
/// </summary>
public static class PropertyExtensions
{
    /// <summary>
    /// Adds all the property services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="propertyServicesOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddPropertyServices(this IServiceCollection services, PropertyServicesOptions propertyServicesOptions)
    {
        ArgumentNullException.ThrowIfNull(propertyServicesOptions);

        services
            .AddPropertyFactories()
            .AddPropertyMaps(propertyServicesOptions.PropertyMapOptions)
            .AddPropertyRepositories();

        return services;
    }
}
