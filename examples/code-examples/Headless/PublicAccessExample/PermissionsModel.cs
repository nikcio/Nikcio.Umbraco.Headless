using HotChocolate;

namespace Code.Examples.Headless.PublicAccessExample;

[GraphQLDescription("Represents a restrict public access settings of a content item.")]
public class PermissionsModel
{
    public PermissionsModel()
    {
        AccessRules = [];
    }

    [GraphQLDescription("Gets the url to the login page.")]
    public string? UrlLogin { get; set; }

    [GraphQLDescription("Gets the url to the error page.")]
    public string? UrlNoAccess { get; set; }

    [GraphQLDescription("Gets the access rules for the restrict public access settings.")]
    public List<AccessRuleModel> AccessRules { get; set; }
}
