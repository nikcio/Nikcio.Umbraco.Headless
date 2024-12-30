using Nikcio.UHeadless.Members;
using Nikcio.UHeadless.Properties;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Members;

public class MemberItem : MemberItemBase
{
    protected IVariationContextAccessor VariationContextAccessor { get; }

    protected IDependencyReflectorFactory DependencyReflectorFactory { get; }

    protected IPublishedMemberCache PublishedMemberCache { get; }

    protected IMemberService MemberService { get; }

    public MemberItem(CreateCommand command) : base(command)
    {
        ArgumentNullException.ThrowIfNull(command);

        VariationContextAccessor = ResolverContext.Service<IVariationContextAccessor>();
        DependencyReflectorFactory = ResolverContext.Service<IDependencyReflectorFactory>();
        PublishedMemberCache = ResolverContext.Service<IPublishedMemberCache>();
        MemberService = ResolverContext.Service<IMemberService>();

    }

    /// <summary>
    /// Gets the name of a member item
    /// </summary>
    [GraphQLDescription("Gets the name of a member item.")]
    public string? Name => PublishedContent?.Name(VariationContextAccessor, Culture);

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
    /// Gets the properties of the member item
    /// </summary>
    [GraphQLDescription("Gets the properties of the member item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
