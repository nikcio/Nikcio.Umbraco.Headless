using HotChocolate;
using Nikcio.UHeadless.Base.Elements.Models;

namespace Nikcio.UHeadless.Media.Models;

/// <summary>
/// Represents a Media item
/// </summary>
[GraphQLDescription("Represents a Media item.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used for a base for media types")]
public interface IMedia : IElement
{
}
