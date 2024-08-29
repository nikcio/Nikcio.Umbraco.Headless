using Nikcio.UHeadless.Base.Properties.Maps;
using Nikcio.UHeadless.Base.TypeModules;
using Nikcio.UHeadless.Media.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Media.TypeModules;

/// <summary>
/// A module to create models to fetch properties based on media types
/// </summary>
public class MediaTypeModule : UmbracoTypeModuleBase<IMediaType, INamedMediaProperties>
{
    private readonly IMediaTypeService _mediaTypeService;

    /// <inheritdoc/>
    public MediaTypeModule(IMediaTypeService mediaTypeService, IPropertyMap propertyMap, IRuntimeState runtimeState) : base(propertyMap, runtimeState)
    {
        _mediaTypeService = mediaTypeService;
    }

    /// <inheritdoc/>
    [Obsolete("Use contructor with all parameters instead.")]
    public MediaTypeModule(IMediaTypeService mediaTypeService, IPropertyMap propertyMap) : base(propertyMap)
    {
        _mediaTypeService = mediaTypeService;
    }

    /// <inheritdoc/>
    protected override IEnumerable<IMediaType> GetContentTypes()
    {
        return _mediaTypeService.GetAll();
    }
}
