{
  "data": {
    "memberByEmail": {
      "properties": {
        "umbracoMemberComments": {
          "value": "",
          "model": "DefaultProperty"
        },
        "umbracoMemberFailedPasswordAttempts": {
          "value": 0,
          "model": "Label"
        },
        "umbracoMemberApproved": {
          "value": false,
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
      "name": "A role one",
      "id": 1183,
      "key": "c0baa0a3-2f2a-4093-a40e-96f579f43912",
      "templateId": -1,
      "parent": null,
      "__typename": "MemberItem"
    }
  }
}