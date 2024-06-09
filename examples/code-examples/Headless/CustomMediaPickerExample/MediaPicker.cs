using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Code.Examples.Headless.CustomMediaPickerExample;

public class MediaPicker : Nikcio.UHeadless.Defaults.Properties.MediaPicker<MediaPickerItem>
{
    public MediaPicker(CreateCommand command) : base(command)
    {
    }

    protected override MediaPickerItem CreateMediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new MediaPickerItem(publishedContent, resolverContext);
    }
    public string? CustomProperty => "Custom value";

    public string? CustomMethod()
    {
        return "Custom method";
    }

    public string? CustomMethodWithParameter(string? parameter)
    {
        return $"Custom method with parameter: {parameter}";
    }

    public string? CustomMethodWithResolverContext(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IHttpContextAccessor httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();

        return $"Custom method with resolver context so you can resolve the services needed: {httpContextAccessor.HttpContext?.Request.Path}";
    }
}

public class MediaPickerItem : Nikcio.UHeadless.Defaults.Properties.MediaPickerItem
{
    public MediaPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext) : base(publishedContent, resolverContext)
    {
    }
    public string? CustomProperty => "Custom value";

    public string? CustomMethod()
    {
        return "Custom method";
    }

    public string? CustomMethodWithParameter(string? parameter)
    {
        return $"Custom method with parameter: {parameter}";
    }

    public string? CustomMethodWithResolverContext(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IHttpContextAccessor httpContextAccessor = resolverContext.Service<IHttpContextAccessor>();

        return $"Custom method with resolver context so you can resolve the services needed: {httpContextAccessor.HttpContext?.Request.Path}";
    }
}
