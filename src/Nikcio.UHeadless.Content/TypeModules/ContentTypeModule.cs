using Nikcio.UHeadless.Base.Properties.Maps;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Base.TypeModules;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Content.TypeModules;

/// <summary>
/// A module to create models to fetch properties based on content types
/// </summary>
public class ContentTypeModule : UmbracoTypeModuleBase<IContentType, INamedContentProperties>
{
    private readonly IContentTypeService _contentTypeService;

    /// <inheritdoc/>
    public ContentTypeModule(IContentTypeService contentTypeService, IPropertyMap propertyMap, IRuntimeState runtimeState) : base(propertyMap, runtimeState)
    {
        _contentTypeService = contentTypeService;
    }

    /// <inheritdoc/>
    [Obsolete("Use contructor with all parameters instead.")]
    public ContentTypeModule(IContentTypeService contentTypeService, IPropertyMap propertyMap) : base(propertyMap)
    {
        _contentTypeService = contentTypeService;
    }

    /// <inheritdoc/>
    protected override IEnumerable<IContentType> GetContentTypes()
    {
        return _contentTypeService.GetAll();
    }
}
