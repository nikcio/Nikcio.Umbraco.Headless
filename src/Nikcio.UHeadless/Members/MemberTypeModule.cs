using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.TypeModules;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Members;

/// <summary>
/// A module to create models to fetch properties based on member types
/// </summary>
internal class MemberTypeModule : UmbracoTypeModuleBase<IMemberType>
{
    private readonly IMemberTypeService _memberTypeService;

    public MemberTypeModule(IMemberTypeService memberTypeService, IPropertyMap propertyMap) : base(propertyMap)
    {
        _memberTypeService = memberTypeService;
    }

    protected override IEnumerable<IMemberType> GetContentTypes()
    {
        return _memberTypeService.GetAll();
    }
}
