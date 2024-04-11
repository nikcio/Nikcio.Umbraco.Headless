using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.TypeModules;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.ContentItems;

/// <summary>
/// A module to create models to fetch properties based on content types
/// </summary>
internal class ContentTypeModule : UmbracoTypeModuleBase<IContentType>
{
    private readonly IContentTypeService _contentTypeService;

    public ContentTypeModule(IContentTypeService contentTypeService, IPropertyMap propertyMap) : base(propertyMap)
    {
        _contentTypeService = contentTypeService;
    }

    protected override IEnumerable<IContentType> GetContentTypes()
    {
        return _contentTypeService.GetAll();
    }
}
