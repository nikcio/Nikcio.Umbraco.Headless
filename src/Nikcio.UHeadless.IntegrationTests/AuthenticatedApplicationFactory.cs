using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.ContentItems;
using Nikcio.UHeadless.Defaults.MediaItems;
using Nikcio.UHeadless.Defaults.Members;

namespace Nikcio.UHeadless.IntegrationTests;

public class AuthenticatedApplicationFactory : ApplicationFactoryBase<TestProject.Program>
{
    public const string ApiKey = "AJvT7FWOPL61h*PFXCsduJbLQKX5Q@w2l9c6VGIzTT4twUhX374dltXfQUwg89lUD&dk@5B^3aYb@QmepnuEARPAUhkAp%czAcQ";

    public override UHeadlessSetup UHeadlessSetup => new AuthenticatedUHeadlessSetup();

    public override string TestDatabaseName => "Default-Tests.sqlite";
}

public class AuthenticatedUHeadlessSetup : UHeadlessSetup
{
    public override Action<UHeadlessOptions> GetSetup()
    {
        return options =>
        {
            options.AddAuth(new()
            {
                ApiKey = AuthenticatedApplicationFactory.ApiKey,
                Secret = "GbLv0IRh*gk7bLVuDHw$3CelL409RHx3Z#0sLvRXhHnE&F@kPBMufdjPqYgzl%d8I52gY83k8Sf4CxbGYbMyFg7lQZ2o^49aL9x"
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
        };
    }
}
