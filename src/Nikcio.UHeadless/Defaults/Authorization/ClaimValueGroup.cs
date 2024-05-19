namespace Nikcio.UHeadless.Defaults.Auth;

public sealed class ClaimValueGroup
{
    public required string GroupName { get; set; }

    public List<AvailableClaimValue> ClaimValues { get; } = [];
}

public sealed class AvailableClaimValue
{
    public required string Name { get; set; }

    public required List<string> Values { get; set; }
}
