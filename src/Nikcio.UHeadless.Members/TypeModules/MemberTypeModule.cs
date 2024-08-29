using Nikcio.UHeadless.Base.Properties.Maps;
using Nikcio.UHeadless.Base.TypeModules;
using Nikcio.UHeadless.Members.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Members.TypeModules;

/// <summary>
/// A module to create models to fetch properties based on member types
/// </summary>
public class MemberTypeModule : UmbracoTypeModuleBase<IMemberType, INamedMemberProperties>
{
    private readonly IMemberTypeService _memberTypeService;

    /// <inheritdoc/>
    public MemberTypeModule(IMemberTypeService memberTypeService, IPropertyMap propertyMap, IRuntimeState runtimeState) : base(propertyMap, runtimeState)
    {
        _memberTypeService = memberTypeService;
    }

    /// <inheritdoc/>
    [Obsolete("Use constructor with all parameters instead.")]
    public MemberTypeModule(IMemberTypeService memberTypeService, IPropertyMap propertyMap) : base(propertyMap)
    {
        _memberTypeService = memberTypeService;
    }

    /// <inheritdoc/>
    protected override IEnumerable<IMemberType> GetContentTypes()
    {
        return _memberTypeService.GetAll();
    }
}
