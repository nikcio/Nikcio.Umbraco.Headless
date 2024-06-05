using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests.TestProject;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
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
    }
}
