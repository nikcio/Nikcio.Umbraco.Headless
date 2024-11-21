using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.MemberItems;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Code.Examples.Headless.CustomMemberItemExample;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class CustomMemberItemExampleQuery : MemberByIdQuery<MemberItem>
{
    [GraphQLName("CustomMemberItemExampleQuery")]
    public override MemberItem? MemberById(
        IResolverContext resolverContext,
        [GraphQLDescription("The id to fetch.")] int id)
    {
        return base.MemberById(resolverContext, id);
    }

    protected override MemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<MemberItem> memberItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);

        return memberItemRepository.GetMemberItem(new Nikcio.UHeadless.Members.MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        });
    }
}
