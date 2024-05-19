using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.IntegrationTests.TestProject;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorFilter<GraphQLErrorFilter>();

builder.Services.ConfigureOptions<ConfigureExamineIndexes>();


IUmbracoBuilder umbracoBuilder = builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers();

if (builder.Environment.IsDevelopment())
{
    umbracoBuilder.AddUHeadless(options =>
    {
        //options.DisableAuthorization = true;

        options.AddAuth(new()
        {
            ApiKey = "uheadless123456789123456789123456789",
            Secret = "uheadless123456789123456789123456789uheadless123456789123456789123456789uheadless123456789123456789123456789",
        });

        options.AddQuery<UtilityClaimGroupsQuery>();

        options.AddDefaults();

        options
            .AddQuery<ContentByRouteQuery>()
            .AddQuery<ContentByContentTypeQuery>()
            .AddQuery<ContentAtRootQuery>()
            .AddQuery<ContentByIdQuery>()
            .AddQuery<ContentByGuidQuery>()
            .AddQuery<ContentByTagQuery>();

        options
            .AddQuery<MediaByContentTypeQuery>()
            .AddQuery<MediaAtRootQuery>()
            .AddQuery<MediaByIdQuery>()
            .AddQuery<MediaByGuidQuery>();

        options
            .AddQuery<FindMembersByDisplayNameQuery>()
            .AddQuery<FindMembersByEmailQuery>()
            .AddQuery<FindMembersByRoleQuery>()
            .AddQuery<FindMembersByUsernameQuery>()
            .AddQuery<MemberByEmailQuery>()
            .AddQuery<MemberByGuidQuery>()
            .AddQuery<MemberByIdQuery>()
            .AddQuery<MemberByUsernameQuery>();

        // This is to allow the long queries we build for getting all the fields for tests.
        options.RequestExecutorBuilder.ModifyParserOptions(parserOptions =>
        {
            parserOptions.MaxAllowedFields = 10000;
        });
    });
}

umbracoBuilder.Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync().ConfigureAwait(false);

app.UseAuthentication();
app.UseAuthorization();

app.MapUHeadless();

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
