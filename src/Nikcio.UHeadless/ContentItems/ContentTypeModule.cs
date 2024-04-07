using Nikcio.UHeadless.Shared.Properties;
using Nikcio.UHeadless.Shared.TypeModules;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.ContentItems;

/// <summary>
/// A module to create models to fetch properties based on content types
/// </summary>
internal class ContentTypeModule : UmbracoTypeModuleBase<IContentType>
{
    private readonly IContentTypeService _contentTypeService;

    /// <inheritdoc/>
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
