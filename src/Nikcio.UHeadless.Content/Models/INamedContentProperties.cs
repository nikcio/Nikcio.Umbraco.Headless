using HotChocolate;

namespace Nikcio.UHeadless.Base.Properties.Models;

/// <summary>
/// Represents the properties that can be queried by the alias name
/// </summary>
[GraphQLDescription("Represents a property that can be queried by the alias name.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a base for content named properties")]
public interface INamedContentProperties : INamedProperties
{
}

/// <summary>
/// Only used to make <see cref="INamedContentProperties"/> possible. Do not use anywhere else!
/// </summary>
internal sealed class NamedContentProperties : INamedContentProperties
{
}