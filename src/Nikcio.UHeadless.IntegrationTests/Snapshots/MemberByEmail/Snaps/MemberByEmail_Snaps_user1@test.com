{
  "data": {
    "memberByEmail": {
      "properties": {
        "umbracoMemberComments": {
          "value": "A user comment",
          "model": "DefaultProperty"
        },
        "umbracoMemberFailedPasswordAttempts": {
          "value": 0,
          "model": "Label"
        },
        "umbracoMemberApproved": {
          "value": true,
          "model": "DefaultProperty"
        },
        "umbracoMemberLockedOut": {
          "value": false,
          "model": "DefaultProperty"
        },
        "umbracoMemberLastLockoutDate": {
          "value": null,
          "model": "Label"
        },
        "umbracoMemberLastLogin": {
          "value": null,
          "model": "Label"
        },
        "umbracoMemberLastPasswordChangeDate": {
          "value": null,
          "model": "Label"
        }
      },
      "name": "user1",
      "id": 1179,
      "key": "91eecdea-fc1e-408d-ac41-545ceefa3dc5",
      "templateId": -1,
      "parent": null,
      "__typename": "MemberItem"
    }
  }
}