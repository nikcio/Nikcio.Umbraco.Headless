using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nikcio.UHeadless.Defaults.Authorization;

public interface IAuthorizationTokenProvider
{
    /// <summary>
    /// Creates a new JWT token
    /// </summary>
    /// <param name="claims">
    /// The claims to include in the token
    /// </param>
    /// <returns></returns>
    JwtSecurityToken CreateToken(params Claim[] claims);

    /// <summary>
    /// Gets the available claims
    /// </summary>
    /// <returns></returns>
    List<ClaimValueGroup> GetAvailableClaims();
}

internal class AuthorizationTokenProvider : IAuthorizationTokenProvider
{
    private readonly UHeadlessAuthorizationOptions _options;

    private static readonly List<ClaimValueGroup> _availableClaims = [];

    private static readonly Lock _availableClaimsLock = new();

    public AuthorizationTokenProvider(UHeadlessAuthorizationOptions options)
    {
        _options = options;
    }

    public JwtSecurityToken CreateToken(params Claim[] claims)
    {
        if (_options == null)
        {
            throw new InvalidOperationException("Authorization options are not set in the UHeadless options using .AddAuth().");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: credentials);

        return token;
    }

    public List<ClaimValueGroup> GetAvailableClaims()
    {
        return _availableClaims;
    }

    internal static void AddAvailableClaimValue(string groupName, AvailableClaimValue claimValue)
    {
        lock (_availableClaimsLock)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException("The group name must be provided", nameof(groupName));
            }

            ArgumentNullException.ThrowIfNull(claimValue);

            ClaimValueGroup? claimValueGroup = _availableClaims.Find(x => x.GroupName == groupName);

            if (claimValueGroup == null)
            {
                claimValueGroup = new ClaimValueGroup
                {
                    GroupName = groupName,
                };

                claimValueGroup.ClaimValues.Add(claimValue);

                _availableClaims.Add(claimValueGroup);
                return;
            }

            AvailableClaimValue? existingValue = claimValueGroup.ClaimValues.Find(value => value.Name == claimValue.Name);

            if (existingValue != null)
            {
                var newValues = claimValue.Values.Except(existingValue.Values).ToList();
                existingValue.Values.AddRange(newValues);
            }
            else
            {
                claimValueGroup.ClaimValues.Add(claimValue);
            }
        }
    }
}
