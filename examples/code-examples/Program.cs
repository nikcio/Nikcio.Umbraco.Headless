using Code.Examples.Headless.PublicAccessExample;
using Code.Examples.Headless.SkybrudRedirectsExample;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .AddUHeadless(options =>
    {
        options.DisableAuthorization = true;

        options.AddDefaults();

        options.AddQuery<PublishAccessExampleQuery>();

        options.AddQuery<SkybrudRedirectsExampleQuery>();

        // Default queries
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
    })
    .Build();

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
