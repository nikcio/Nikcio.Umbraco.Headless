using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Authorization;
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
        options.AddAuth(new()
        {
            ApiKey = "uheadless123456789123456789123456789",
            Secret = "uheadless123456789123456789123456789uheadless123456789123456789123456789uheadless123456789123456789123456789",
        });

        options.AddDefaults();

        options.AddQuery<UtilityClaimGroupsQuery>();

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
