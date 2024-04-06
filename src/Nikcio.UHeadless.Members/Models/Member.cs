using Nikcio.UHeadless.Base.Elements.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Members.Commands;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Members.Models;

/// <summary>
/// A base for member
/// </summary>
/// <typeparam name="TProperty"></typeparam>
public abstract class Member<TProperty> : Element<TProperty>, IMember
    where TProperty : IProperty
{
    /// <inheritdoc/>
    protected Member(CreateMember createMember, IPropertyFactory<TProperty> propertyFactory) :
        base(
            createMember?.CreateElement ?? throw new ArgumentNullException(nameof(createMember)),
            propertyFactory)
    {
        MemberItem = createMember.Member;
    }

    /// <summary>
    /// The content
    /// </summary>
    protected virtual IPublishedContent? MemberItem { get; }
}
