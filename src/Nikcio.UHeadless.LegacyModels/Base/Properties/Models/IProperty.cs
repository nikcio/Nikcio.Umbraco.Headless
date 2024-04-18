using HotChocolate;

namespace Nikcio.UHeadless.Base.Properties.Models;

/// <summary>
/// Represents a property
/// </summary>
[GraphQLDescription("Represents a property.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is used for a base for all properties")]
[Obsolete("Convert to using typed properties instead")]
public interface IProperty
{
}
