{
  "data": {
    "contentByAbsoluteRoute": {
      "id": 1148,
      "key": "dcf14fa0-dfc3-4108-8c9b-a4e9f77c0c29",
      "path": "-1,1148",
      "name": "Site 2",
      "creatorId": -1,
      "writerId": -1,
      "properties": [],
      "itemType": "CONTENT",
      "level": 1,
      "parent": null,
      "redirect": null,
      "sortOrder": 2,
      "templateId": null,
      "url": "http://site-2.com/",
      "urlSegment": "site-2",
      "absoluteUrl": "http://site-2.com/",
      "children": [
        {
          "name": "Page 1",
          "creatorId": -1,
          "writerId": -1,
          "properties": [
            {
              "alias": "blockGrid"
            },
            {
              "alias": "blockList"
            },
            {
              "alias": "checkboxList"
            },
            {
              "alias": "colorPicker"
            },
            {
              "alias": "contentPicker"
            },
            {
              "alias": "datePicker"
            },
            {
              "alias": "datePickerWithTime"
            },
            {
              "alias": "decimal"
            },
            {
              "alias": "dropdown"
            },
            {
              "alias": "emailAddress"
            },
            {
              "alias": "eyeDropperColorPicker"
            },
            {
              "alias": "article"
            },
            {
              "alias": "audio"
            },
            {
              "alias": "file"
            },
            {
              "alias": "svg"
            },
            {
              "alias": "video"
            },
            {
              "alias": "imageCropper"
            },
            {
              "alias": "markdown"
            },
            {
              "alias": "imageMediaPicker"
            },
            {
              "alias": "mediaPicker"
            },
            {
              "alias": "multipleImageMediaPicker"
            },
            {
              "alias": "multipleMediaPicker"
            },
            {
              "alias": "mediaPickerLegacy"
            },
            {
              "alias": "multipleMediaPickerLegacy"
            },
            {
              "alias": "memberGroupPicker"
            },
            {
              "alias": "memberPicker"
            },
            {
              "alias": "multinodeTreepicker"
            },
            {
              "alias": "multiUrlPicker"
            },
            {
              "alias": "numeric"
            },
            {
              "alias": "radiobox"
            },
            {
              "alias": "repeatableTextstrings"
            },
            {
              "alias": "richtext"
            },
            {
              "alias": "slider"
            },
            {
              "alias": "tags"
            },
            {
              "alias": "textarea"
            },
            {
              "alias": "textstring"
            },
            {
              "alias": "trueOrFalse"
            },
            {
              "alias": "userPicker"
            }
          ],
          "itemType": "CONTENT",
          "level": 2,
          "parent": {
            "name": "Site 2"
          },
          "redirect": null,
          "sortOrder": 0,
          "templateId": null,
          "url": "http://site-2.com/page-1/",
          "urlSegment": "page-1",
          "absoluteUrl": "http://site-2.com/page-1/"
        }
      ]
    }
  }
}