using HotChocolate;
using Nikcio.UHeadless.Base.Elements.Models;

namespace Nikcio.UHeadless.Members.Models;

/// <summary>
/// Represents a member
/// </summary>
[GraphQLDescription("Represents a member.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a base for member types")]
public interface IMember : IElement
{
}
