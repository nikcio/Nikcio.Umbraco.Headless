namespace Code.Examples.Headless.PublicAccessExample;

[GraphQLDescription("Represents an access rule for the restrict public access settings.")]
public class AccessRuleModel
{
    public AccessRuleModel(string? ruleType, string? ruleValue)
    {
        RuleType = ruleType ?? string.Empty;
        RuleValue = ruleValue ?? string.Empty;
    }

    [GraphQLDescription("Gets the type of protection to grant access to the content item.")]
    public string RuleType { get; set; }

    [GraphQLDescription("Gets the name of who has access to the content item.")]
    public string RuleValue { get; set; }
}
