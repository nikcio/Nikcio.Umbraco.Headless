using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests;

public class UnAuthenticatedApplicationFactory : ApplicationFactoryBase<TestProject.Program>
{
    public override UHeadlessSetup UHeadlessSetup => new UnAuthenticatedUHeadlessSetup();

    public override string TestDatabaseName => "Default-Tests.sqlite";
}

public class UnAuthenticatedUHeadlessSetup : UHeadlessSetup
{
    public override Action<UHeadlessOptions> GetSetup()
    {
        return options =>
        {
            options.DisableAuthorization = true;

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
        };
    }
}
