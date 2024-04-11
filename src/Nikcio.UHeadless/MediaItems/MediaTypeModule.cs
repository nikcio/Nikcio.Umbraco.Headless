using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.TypeModules;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.MediaItems;

/// <summary>
/// A module to create models to fetch properties based on media types
/// </summary>
internal class MediaTypeModule : UmbracoTypeModuleBase<IMediaType>
{
    private readonly IMediaTypeService _mediaTypeService;

    public MediaTypeModule(IMediaTypeService mediaTypeService, IPropertyMap propertyMap) : base(propertyMap)
    {
        _mediaTypeService = mediaTypeService;
    }

    protected override IEnumerable<IMediaType> GetContentTypes()
    {
        return _mediaTypeService.GetAll();
    }
}
