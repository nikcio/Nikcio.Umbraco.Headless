{
  "data": {
    "contentByAbsoluteRoute": {
      "id": 1146,
      "key": "eadd5be4-456c-4a7d-8c4a-2f7ead9c8ecf",
      "path": "-1,1146",
      "name": "Site 1",
      "creatorId": -1,
      "writerId": -1,
      "properties": [],
      "itemType": "CONTENT",
      "level": 1,
      "parent": null,
      "redirect": null,
      "sortOrder": 0,
      "templateId": null,
      "url": "http://site-1.com/",
      "urlSegment": "site-1",
      "absoluteUrl": "http://site-1.com/",
      "children": [
        {
          "name": "Homepage",
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
            "name": "Site 1"
          },
          "redirect": null,
          "sortOrder": 0,
          "templateId": null,
          "url": "http://site-1.com/homepage/",
          "urlSegment": "homepage",
          "absoluteUrl": "http://site-1.com/homepage/"
        },
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
            "name": "Site 1"
          },
          "redirect": null,
          "sortOrder": 1,
          "templateId": null,
          "url": "http://site-1.com/page-1/",
          "urlSegment": "page-1",
          "absoluteUrl": "http://site-1.com/page-1/"
        },
        {
          "name": "Page 2",
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
            "name": "Site 1"
          },
          "redirect": null,
          "sortOrder": 2,
          "templateId": null,
          "url": "http://site-1.com/page-2/",
          "urlSegment": "page-2",
          "absoluteUrl": "http://site-1.com/page-2/"
        },
        {
          "name": "Collection of pages",
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
            "name": "Site 1"
          },
          "redirect": null,
          "sortOrder": 3,
          "templateId": null,
          "url": "http://site-1.com/collection-of-pages/",
          "urlSegment": "collection-of-pages",
          "absoluteUrl": "http://site-1.com/collection-of-pages/"
        }
      ]
    }
  }
}