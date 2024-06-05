using Code.Examples.Headless.CustomContentItemExample;
using Code.Examples.Headless.PublicAccessExample;
using Code.Examples.Headless.SkybrudRedirectsExample;
using Code.Examples.Headless.UrlTrackerExample;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;

namespace Code.Examples;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IUmbracoBuilder umbracoBuilder = builder.CreateUmbracoBuilder()
            .AddBackOffice()
            .AddWebsite()
            .AddDeliveryApi()
            .AddComposers();

        // This ensures the integration tests control the UHeadless setup
        if (builder.Environment.IsDevelopment()) 
        {
            umbracoBuilder.AddUHeadless(options =>
            {
                options.DisableAuthorization = true;

                options.AddDefaults();

                options.AddQuery<PublishAccessExampleQuery>();
                options.AddQuery<SkybrudRedirectsExampleQuery>();
                options.AddQuery<UrlTrackerExampleQuery>();
                options.AddMutation<TrackErrorStatusCodeMutation>();
                options.AddQuery<CustomContentItemExampleQuery>();

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
