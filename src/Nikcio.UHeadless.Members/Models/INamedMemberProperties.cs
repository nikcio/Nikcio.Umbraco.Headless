using HotChocolate;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Nikcio.UHeadless.Members.Models;

/// <summary>
/// Represents the properties that can be queried by the alias name
/// </summary>
[GraphQLDescription("Represents a property that can be queried by the alias name.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a base for member named properties")]
public interface INamedMemberProperties : INamedProperties
{
}

/// <summary>
/// Only used to make <see cref="INamedMemberProperties"/> possible. Do not use anywhere else!
/// </summary>
public sealed class NamedMemberProperties : INamedMemberProperties
{
}
