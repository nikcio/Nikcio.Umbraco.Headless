using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Nikcio.UHeadless.Base.Properties.Factories;

/// <summary>
/// A factory to create property values
/// </summary>
[Obsolete("Convert to using typed properties instead")]
public interface IPropertyValueFactory
{
    /// <summary>
    /// Get a property value
    /// </summary>
    /// <param name="createPropertyValue"></param>
    /// <returns></returns>
    PropertyValue? GetPropertyValue(CreatePropertyValue createPropertyValue);
}
