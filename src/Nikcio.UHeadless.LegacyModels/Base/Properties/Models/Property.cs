using HotChocolate;
using Nikcio.UHeadless.Base.Properties.Commands;

namespace Nikcio.UHeadless.Base.Properties.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a property.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This is the naming for this data given by Umbraco and would be confusing to change.")]
[Obsolete("Convert to using typed properties instead")]
public abstract class Property : IProperty
{
    /// <inheritdoc/>
    protected Property(CreateProperty _)
    {
    }
}
