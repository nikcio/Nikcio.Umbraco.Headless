using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Nikcio.UHeadless.Defaults.Authorization;

internal class ApiKeySchemeHandler : AuthenticationHandler<ApiKeySchemeOptions>
{
    [Obsolete("ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.")]
    public ApiKeySchemeHandler(IOptionsMonitor<ApiKeySchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    public ApiKeySchemeHandler(IOptionsMonitor<ApiKeySchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.TryGetValue(DefaultHeaders.ApiKey, out StringValues apiKey))
        {
            bool isApiKeyValid = await Options.IsApiKeyValid.Invoke(apiKey.ToString(), Context.RequestServices).ConfigureAwait(false);

            if (isApiKeyValid)
            {
                AuthenticationTicket ticket = await Options.CreateAuthenticationTicket.Invoke(apiKey.ToString(), Scheme, Context.RequestServices).ConfigureAwait(false);
                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("Invalid API key");
            }
        }

        return AuthenticateResult.NoResult();
    }
}

/// <summary>
/// Options for the Api Key authentication scheme
/// </summary>
public class ApiKeySchemeOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Function to validate the Api key
    /// </summary>
    public IsApiKeyValidAsync IsApiKeyValid { get; set; } = (apiKey, _) => Task.FromResult(false);

    /// <summary>
    /// Function to create an authentication ticket
    /// </summary>
    /// <remarks>
    /// Takes the Api Key, the authentication scheme and a service provider as input and returns an authentication ticket.
    /// </remarks>
    public CreateAuthenticationTicketAsync CreateAuthenticationTicket { get; set; } = (apiKey, scheme, _) =>
    {
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity([], scheme.Name));
        return Task.FromResult(new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), scheme.Name));
    };
}

/// <summary>
/// Function to validate the Api key
/// </summary>
/// <param name="apiKey">The provided Api key</param>
/// <param name="serviceProvider">A service provider</param>
/// <returns></returns>
public delegate Task<bool> IsApiKeyValidAsync(string apiKey, IServiceProvider serviceProvider);

/// <summary>
/// Function to create an authentication ticket
/// </summary>
/// <param name="apiKey">The provided Api key</param>
/// <param name="scheme">The current authentication scheme</param>
/// <param name="serviceProvider">A service provider</param>
/// <returns></returns>
public delegate Task<AuthenticationTicket> CreateAuthenticationTicketAsync(string apiKey, AuthenticationScheme scheme, IServiceProvider serviceProvider);

