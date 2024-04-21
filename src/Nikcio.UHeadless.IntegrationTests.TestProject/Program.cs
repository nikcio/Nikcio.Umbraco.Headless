using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.IntegrationTests.TestProject;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorFilter<GraphQLErrorFilter>();

builder.Services.ConfigureOptions<ConfigureExamineIndexes>();

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .AddUHeadless(options =>
    {
        options.PropertyMap.AddDefaults();

        options.RequestExecutorBuilder
            .AddTypeExtension<ContentByRouteQuery>()
            .AddTypeExtension<ContentByContentTypeQuery>()
            .AddTypeExtension<ContentAtRootQuery>()
            .AddTypeExtension<ContentByIdQuery>()
            .AddTypeExtension<ContentByGuidQuery>()
            .AddTypeExtension<ContentByTagQuery>();

        options.RequestExecutorBuilder
            .AddTypeExtension<MediaByContentTypeQuery>()
            .AddTypeExtension<MediaAtRootQuery>()
            .AddTypeExtension<MediaByIdQuery>()
            .AddTypeExtension<MediaByGuidQuery>();

        options.RequestExecutorBuilder
            .AddTypeExtension<FindMembersByDisplayNameQuery>()
            .AddTypeExtension<FindMembersByEmailQuery>()
            .AddTypeExtension<FindMembersByRoleQuery>()
            .AddTypeExtension<FindMembersByUsernameQuery>()
            .AddTypeExtension<MemberByEmailQuery>()
            .AddTypeExtension<MemberByGuidQuery>()
            .AddTypeExtension<MemberByIdQuery>()
            .AddTypeExtension<MemberByUsernameQuery>();

        // This is to allow the long queries we build for getting all the fields for tests.
        options.RequestExecutorBuilder.ModifyParserOptions(parserOptions =>
        {
            parserOptions.MaxAllowedFields = 10000;
        });
    })
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync().ConfigureAwait(false);

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

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
