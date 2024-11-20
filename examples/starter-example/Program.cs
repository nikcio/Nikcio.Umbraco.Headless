using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Extensions;
using Nikcio.UHeadless;

/*
 * This setup showcases a minimal setup for UHeadless in an Umbraco application.
 * 
 * In this example we have:
 * - Enabled authentication using configuration values for the API key and secret.
 * - Disabled the GraphQL IDE in production.
 * - Added a single query for fetching content by route.
 * - Removed Umbraco's services and endpoints for rendering a website as this will only be used as a headless setup. 
 *   - .AddWebsite() is needed for the install screen to load if you're not using UnattendedInstall.
 */

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// This is needed for the automatic persisted query pipeline when using in-memory query storage
builder.Services.AddMemoryCache();

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .AddUHeadless(options =>
    {
        options.AddAuth(new()
        {
            ApiKey = builder.Configuration.GetValue<string>("UHeadless:ApiKey") ?? throw new InvalidOperationException("No value for UHeadless:ApiKey was found"),
            Secret = builder.Configuration.GetValue<string>("UHeadless:Secret") ?? throw new InvalidOperationException("No value for UHeadless:Secret was found"),
        });

        options.AddDefaults();

        options.AddQuery<starter_example.Headless.ContentByRouteQuery>();

        // This adds the automatic persisted query pipeline to the request executor
        options.RequestExecutorBuilder
            .UseAutomaticPersistedQueryPipeline()
            .AddInMemoryQueryStorage();
    })
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync().ConfigureAwait(false);

app.UseAuthentication();
app.UseAuthorization();

GraphQLEndpointConventionBuilder graphQLEndpointBuilder = app.MapUHeadless();

// Only enable the GraphQL IDE in development
if (!builder.Environment.IsDevelopment())
{
    graphQLEndpointBuilder.WithOptions(new GraphQLServerOptions()
    {
        Tool =
        {
            Enable = false,
        }
    });
}

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
    });

await app.RunAsync().ConfigureAwait(false);
