---
title: Member Queries
description: Learn how to query members in Nikcio.UHeadless.
---

The Nikcio.UHeadless package provides various member queries that allow you to retrieve member data from Umbraco CMS.
The Nikcio.UHeadless package provides various media queries that allow you to retrieve media items in different ways from Umbraco CMS.

## Queries

You can add any query to the UHeadless options as seen here:

```csharp
.AddUHeadless(options =>
{
    options.AddQuery<MemberByGuidQuery>();
})
```

The following content queries are available:

| Query class Name              | Description                                 | Needed claim values                                         |
|-------------------------------|---------------------------------------------|-------------------------------------------------------------|
| FindMembersByDisplayNameQuery | Finds members by display name.              | find.members.by.display.name.query or global.member.read    |
| FindMembersByEmailQuery       | Finds members by email.                     | find.members.by.email.query or global.member.read           |
| FindMembersByRoleQuery        | Finds members by role.                      | find.members.by.role.query or global.member.read            |
| FindMembersByUsernameQuery    | Finds members by username.                  | find.members.by.username.query or global.member.read        |
| MemberByEmailQuery            | Gets a member by email.                     | member.by.email.query or global.member.read                 |
| MemberByGuidQuery             | Gets a member by Guid.                      | member.by.guid.query or global.member.read                  |
| MemberByIdQuery               | Gets a member by id.                        | member.by.id.query or global.member.read                    |
| MemberByUsernameQuery         | Gets a member by username.                  | member.by.username.query or global.member.read              |

You can explore these queries and their parameters in the UI provided at `/graphql` when you have added them to the UHeadless options as seen above.

The claim values are needed when having authorization enabled. You can read more about authorization and how to create tokens in the [Security Considerations](../security) section.

A special case for claim values are for the member picker editor. To access the data of the member picker you will need one of the following claim values: `property.values.member.picker` or `global.member.read`.

## Next steps

- [Building your property query](./properties)