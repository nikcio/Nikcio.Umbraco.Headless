{
  "data": {
    "memberByEmail": {
      "properties": {
        "nestedContent": {
          "members": [
            {
              "name": "user1",
              "id": 1179,
              "key": "91eecdea-fc1e-408d-ac41-545ceefa3dc5"
            }
          ],
          "model": "MemberPicker"
        },
        "umbracoMemberComments": {
          "value": "Test member",
          "model": "DefaultProperty"
        }
      },
      "name": "member 2",
      "id": 1181,
      "key": "1ee7e46f-372e-4cf8-b7f4-c8c87abc6ce2",
      "templateId": -1,
      "parent": null,
      "__typename": "MemberItem"
    }
  }
}