using Code.Examples.Headless.CustomContentItemExample;
using Code.Examples.Headless.PublicAccessExample;
using Code.Examples.Headless.SkybrudRedirectsExample;
using Code.Examples.Headless.UrlTrackerExample;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests;

public class CodeExamplesApplicationFactory : ApplicationFactoryBase<Code.Examples.Program>
{
    public override UHeadlessSetup UHeadlessSetup => new CodeExamplesUHeadlessSetup();

    public override string TestDatabaseName => "code-examples.sqlite";
}

public class CodeExamplesUHeadlessSetup : UHeadlessSetup
{
    public override Action<UHeadlessOptions> GetSetup()
    {
        return options =>
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
        };
    }
}
