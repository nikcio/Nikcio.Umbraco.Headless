using HotChocolate.Execution.Configuration;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Extensions;
using Nikcio.UHeadless.IntegrationTests.TestProject;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorFilter<GraphQLErrorFilter>();

builder.Services.ConfigureOptions<ConfigureExamineIndexes>();

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .AddUHeadless(new()
    {
        PropertyServicesOptions = new()
        {
            PropertyMapOptions = new()
            {
                PropertyMappings = []
            },
        },
        TracingOptions = new()
        {
            TimestampProvider = null,
            TracingPreference = HotChocolate.Execution.Options.TracingPreference.Never,
        },
        UHeadlessGraphQLOptions = new()
        {
            GraphQLExtensions = (IRequestExecutorBuilder builder) =>
            {
                builder.AddTypeExtension<ContentByRouteQuery>();
                builder.AddTypeExtension<ContentByContentTypeQuery>();
                builder.AddTypeExtension<ContentAtRootQuery>();
                builder.AddTypeExtension<ContentByIdQuery>();
                builder.AddTypeExtension<ContentByGuidQuery>();
                builder.AddTypeExtension<ContentByTagQuery>();

                builder.AddTypeExtension<MediaByContentTypeQuery>();
                builder.AddTypeExtension<MediaAtRootQuery>();
                builder.AddTypeExtension<MediaByIdQuery>();
                builder.AddTypeExtension<MediaByGuidQuery>();

                return builder;
            },
        },
    })
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync().ConfigureAwait(false);

app.UseAuthentication();
app.UseAuthorization();


app.MapUHeadlessGraphQLEndpoint(new()
{
    CorsPolicy = null,
    GraphQLPath = "/graphql",
    GraphQLServerOptions = new()
    {
        Tool =
        {
            Enable = true
        }
    }
});

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync().ConfigureAwait(false);

public partial class Program
{
    private Program()
    {

    }
}
