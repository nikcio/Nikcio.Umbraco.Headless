using HotChocolate;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.Reflection;
using Nikcio.UHeadless.Members;

namespace Nikcio.UHeadless.Defaults.Members;

public class MemberItem : MemberBase
{
    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public MemberItem(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory) : base(command)
    {
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);
        ArgumentNullException.ThrowIfNull(command);

        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <summary>
    /// Gets the name of a member item
    /// </summary>
    [GraphQLDescription("Gets the name of a member item.")]
    public string? Name => PublishedContent?.Name;

    /// <summary>
    /// Gets the id of a member item
    /// </summary>
    [GraphQLDescription("Gets the id of a member item.")]
    public int? Id => PublishedContent?.Id;

    /// <summary>
    /// Gets the key of a member item
    /// </summary>
    [GraphQLDescription("Gets the key of a member item.")]
    public Guid? Key => PublishedContent?.Key;

    /// <summary>
    /// Gets the identifier of the template to use to render the member item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the template to use to render the member item.")]
    public int? TemplateId => PublishedContent?.TemplateId;

    /// <summary>
    /// Gets the date the member item was last updated
    /// </summary>
    [GraphQLDescription("Gets the date the member item was last updated.")]
    public DateTime? UpdateDate => PublishedContent?.UpdateDate;

    /// <summary>
    /// Gets the parent of the member item
    /// </summary>
    [GraphQLDescription("Gets the parent of the member item.")]
    public MemberItem? Parent => PublishedContent?.Parent != null ? CreateMember<MemberItem>(new CreateCommand()
    {
        PublishedContent = PublishedContent.Parent,
        ResolverContext = ResolverContext,
    }, _dependencyReflectorFactory) : default;

    /// <summary>
    /// Gets the properties of the member item
    /// </summary>
    [GraphQLDescription("Gets the properties of the member item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
