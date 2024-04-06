using Nikcio.UHeadless.Base.Elements.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Media.Commands;

namespace Nikcio.UHeadless.Media.Models;

/// <summary>
/// A base for media
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "This is because of the Umbraco naming and it also causes the namespace naming.")]
public abstract class Media<TProperty> : Element<TProperty>, IMedia
    where TProperty : IProperty
{
    /// <inheritdoc/>
    protected Media(CreateMedia createMedia, IPropertyFactory<TProperty> propertyFactory) :
        base(
            createMedia?.CreateElement ?? throw new ArgumentNullException(nameof(createMedia)),
            propertyFactory
        )
    {
    }
}
