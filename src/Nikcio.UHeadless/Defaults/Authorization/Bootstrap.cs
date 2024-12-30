using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Nikcio.UHeadless.Defaults.Authorization;

internal static class Bootstrap
{
    public static UHeadlessOptions AddAuthInternal(this UHeadlessOptions options)
    {
        if (options.AuthorizationOptions == null)
        {
            throw new InvalidOperationException("AuthorizationOptions must be set in UHeadlessOptions using .AddAuth().");
        }

        options.UmbracoBuilder.Services.AddScoped<IAuthorizationTokenProvider, AuthorizationTokenProvider>();

        options.UmbracoBuilder.Services.AddAuthentication()
            .AddJwtBearer(DefaultAuthenticationSchemes.UHeadless, configure =>
            {
                configure.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.TryGetValue(DefaultHeaders.Token, out StringValues token))
                        {
                            string bearerToken = token.ToString();
                            context.Token = bearerToken.StartsWith("Bearer ", StringComparison.Ordinal) ? bearerToken[7..] : bearerToken;
                        }
                        else
                        {
                            context.NoResult();
                        }

                        return Task.CompletedTask;
                    }
                };

                configure.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = options.AuthorizationOptions.Issuer,
                    ValidAudience = options.AuthorizationOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.AuthorizationOptions.Secret))
                };
            })
            .AddScheme<ApiKeySchemeOptions, ApiKeySchemeHandler>(DefaultAuthenticationSchemes.UHeadlessApiKey, configure =>
            {
                configure.IsApiKeyValid = (apiKey, serviceProvider) =>
                {
                    UHeadlessAuthorizationOptions options = serviceProvider.GetRequiredService<UHeadlessAuthorizationOptions>();
                    return Task.FromResult(apiKey == options.ApiKey);
                };
            });

        options.AddMutation<CreateTokenMutation>();

        return options;
    }
}
