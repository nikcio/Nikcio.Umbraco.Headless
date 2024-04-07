namespace Nikcio.UHeadless.Base.Elements.Models;

/// <summary>
/// Represents a element item
/// </summary>
[GraphQLDescription("Represents a element item.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a base interface to match custom types made by package consumers and also internally for content, media and member types.")]
[Obsolete("We have moved to use the Nikcio.UHeadless.Base.Elements.Models.Element abstract class for methods and classes which depend on this interface. If you're using this you should move to use the abstract class instead.")]
public interface IElement
{
}
