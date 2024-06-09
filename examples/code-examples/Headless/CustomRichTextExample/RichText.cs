using HotChocolate;
using HotChocolate.Resolvers;

namespace Code.Examples.Headless.CustomRichTextExample;

[GraphQLName("CustomRichText")]
public class RichText : Nikcio.UHeadless.Defaults.Properties.RichText
{
    public RichText(CreateCommand command) : base(command)
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
