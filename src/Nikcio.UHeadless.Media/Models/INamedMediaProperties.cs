using HotChocolate;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Nikcio.UHeadless.Media.Models;

/// <summary>
/// Represents the properties that can be queried by the alias name
/// </summary>
[GraphQLDescription("Represents a property that can be queried by the alias name.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a base for the media named properties")]
public interface INamedMediaProperties : INamedProperties
{
}

/// <summary>
/// Only used to make <see cref="INamedMediaProperties"/> possible. Do not use anywhere else!
/// </summary>
internal sealed class NamedMediaProperties : INamedMediaProperties
{
}
