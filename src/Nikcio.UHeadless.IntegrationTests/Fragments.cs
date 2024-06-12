namespace Nikcio.UHeadless.IntegrationTests;

public static class Fragments
{
    /// <summary>
    /// This is meant to select as many properties as possible to give the best image of changes in the output from the tests.
    /// </summary>
    public const string TypedProperties = """
        fragment typedProperties on TypedProperties {
          ... on IBlockGridEditor {
            blockGrid {
              model
              gridColumns
              blocks {
                contentAlias
                settingsAlias
                rowSpan
                columnSpan
                contentProperties {
                  ...typedBlockGridContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
              }
            }
          }
          ... on IEyeDropperColorPickerEditor {
            eyeDropperColorPicker {
              value
              model
              __typename
            }
          }
          ... on IEyeDropperColorPickerEditorCulture {
            eyeDropperColorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IFileUpload {
            article {
              value
              model
              __typename
            }
            audio {
              value
              model
              __typename
            }
            file {
              value
              model
              __typename
            }
            svg {
              value
              model
              __typename
            }
            video {
              value
              model
              __typename
            }
          }
          ... on IFileUploadCulture {
            articleCulture {
              value
              model
              __typename
            }
            audioCulture {
              value
              model
              __typename
            }
            fileCulture {
              value
              model
              __typename
            }
            svgCulture {
              value
              model
              __typename
            }
            videoCulture {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditor {
            imageCropper {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditorCulture {
            imageCropperCulture {
              value
              model
              __typename
            }
          }
          ... on IMarkdownEditor {
            markdown {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMarkdownEditorCulture {
            markdownCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMediaPickerEditor {
            imageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMediaPickerEditorCulture {
            imageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditor {
            blockList {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  ...typedBlockListContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockListSettings
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditorCulture {
            blockListCulture {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  ...typedBlockListContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockListSettings
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditor {
            memberGroupPicker {
              value
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditorCulture {
            memberGroupPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IMemberPickerEditor {
            memberPicker {
              members {
                properties {
                  ...typedMemberProperties
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberPickerEditorCulture {
            memberPickerCulture {
              members {
                properties {
                  ...typedMemberProperties
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditor {
            multinodeTreepicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedContentProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditorCulture {
            multinodeTreepickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedContentProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditor {
            multiUrlPicker {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedContentProperties
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditorCulture {
            multiUrlPickerCulture {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedContentProperties
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on INumericEditor {
            numeric {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditor {
            radiobox {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditorCulture {
            radioboxCulture {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditor {
            checkboxList {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditorCulture {
            checkboxListCulture {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditor {
            repeatableTextstrings {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditorCulture {
            repeatableTextstringsCulture {
              value
              model
              __typename
            }
          }
          ... on IRichtextEditor {
            richtext {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IRichtextEditorCulture {
            richtextCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on ISliderEditor {
            slider {
              value
              model
              __typename
            }
          }
          ... on ISliderEditorCulture {
            sliderCulture {
              value
              model
              __typename
            }
          }
          ... on ITagsEditor {
            tags {
              value
              model
              __typename
            }
          }
          ... on ITagsEditorCulture {
            tagsCulture {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditor {
            textarea {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditorCulture {
            textareaCulture {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditor {
            textstring {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditorCulture {
            textstringCulture {
              value
              model
              __typename
            }
          }
          ... on IToggleEditor {
            trueOrFalse {
              value
              model
              __typename
            }
          }
          ... on IToggleEditorCulture {
            trueOrFalseCulture {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditor {
            userPicker {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditorCulture {
            userPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditor {
            colorPicker {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditorCulture {
            colorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IContentPickerEditor {
            contentPicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedContentProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IContentPickerEditorCulture {
            contentPickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedContentProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on DatePickerEditor {
            datePicker {
              value
              model
              __typename
            }
            datePickerWithTime {
              value
              model
              __typename
            }
          }
          ... on DatePickerEditorCulture {
            datePickerCulture {
              value
              model
              __typename
            }
            datePickerWithTimeCulture {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditor {
            decimal {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditorCulture {
            decimalCulture {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditor {
            dropdown {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditorCulture {
            dropdownCulture {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditor {
            emailAddress {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditorCulture {
            emailAddressCulture {
              value
              model
              __typename
            }
          }
          __typename
        }
        """ + TypedContentProperties + TypedImageProperties + TypedBlockGridContent + TypedBlockGridSettings + TypedBlockListContent + TypedBlockListSettings + TypedMemberProperties;

    /// <summary>
    /// Used to select properties from the custom media type.
    /// </summary>
    public const string CustomMediaType = """
        fragment customMediaType on ICustomMediaType {
          eyeDropperColorPicker {
            value
            model
            __typename
          }
          article {
            value
            model
            __typename
          }
          audio {
            value
            model
            __typename
          }
          file {
            value
            model
            __typename
          }
          video {
            value
            model
            __typename
          }
          imageCropper {
            value
            model
            __typename
          }
          imageMediaPicker {
            mediaItems {
              url(urlMode: ABSOLUTE)
              properties {
                ...typedImageProperties
                __typename
              }
              urlSegment
              name
              id
              key
              __typename
            }
            model
            __typename
          }
          blockList {
            blocks {
              contentAlias
              settingsAlias
              contentProperties {
                ...typedBlockListContent
                __typename
              }
              settingsProperties {
                ...typedBlockListSettings
                __typename
              }
              __typename
            }
            model
            __typename
          }
          memberGroupPicker {
            value
            model
            __typename
          }
          memberPicker {
            members {
              properties {
                ...typedMemberProperties
                __typename
              }
              name
              id
              key
              __typename
            }
            model
            __typename
          }
          multinodeTreepicker {
            items {
              url(urlMode: ABSOLUTE)
              properties {
                ...typedContentProperties
                __typename
              }
              urlSegment
              name
              id
              key
              __typename
            }
            model
            __typename
          }
          multiUrlPicker {
            links {
              url(urlMode: ABSOLUTE)
              properties {
                ...typedContentProperties
                __typename
              }
              urlSegment
              target
              type
              name
              id
              key
              __typename
            }
            model
            __typename
          }
          numeric {
            value
            model
            __typename
          }
          radiobox {
            value
            model
            __typename
          }
          checkboxList {
            value
            model
            __typename
          }
          slider {
            value
            model
            __typename
          }
          tags {
            value
            model
            __typename
          }
          textarea {
            value
            model
            __typename
          }
          textstring {
            value
            model
            __typename
          }
          userPicker {
            value
            model
            __typename
          }
          contentPicker {
            items {
              url(urlMode: ABSOLUTE)
              properties {
                ...typedContentProperties
                __typename
              }
              urlSegment
              name
              id
              key
              __typename
            }
            model
            __typename
          }
          datePickerWithTime {
            value
            model
            __typename
          }
          decimal {
            value
            model
            __typename
          }
          emailAddress {
            value
            model
            __typename
          }
        }
        """ + TypedContentProperties + TypedImageProperties + TypedBlockGridContent + TypedBlockGridSettings + TypedBlockListContent + TypedBlockListSettings + TypedMemberProperties;

    private const string TypedContentProperties = """
        fragment typedContentProperties on TypedProperties {
          ... on IBlockGridEditor {
            blockGrid {
              model
              gridColumns
              blocks {
                contentAlias
                settingsAlias
                rowSpan
                columnSpan
                contentProperties {
                  ...typedBlockGridContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
              }
            }
          }
          ... on IEyeDropperColorPickerEditor {
            eyeDropperColorPicker {
              value
              model
              __typename
            }
          }
          ... on IEyeDropperColorPickerEditorCulture {
            eyeDropperColorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IFileUpload {
            article {
              value
              model
              __typename
            }
            audio {
              value
              model
              __typename
            }
            file {
              value
              model
              __typename
            }
            svg {
              value
              model
              __typename
            }
            video {
              value
              model
              __typename
            }
          }
          ... on IFileUploadCulture {
            articleCulture {
              value
              model
              __typename
            }
            audioCulture {
              value
              model
              __typename
            }
            fileCulture {
              value
              model
              __typename
            }
            svgCulture {
              value
              model
              __typename
            }
            videoCulture {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditor {
            imageCropper {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditorCulture {
            imageCropperCulture {
              value
              model
              __typename
            }
          }
          ... on IMarkdownEditor {
            markdown {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMarkdownEditorCulture {
            markdownCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMediaPickerEditor {
            imageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMediaPickerEditorCulture {
            imageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditor {
            blockList {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  ...typedBlockListContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockListSettings
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditorCulture {
            blockListCulture {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  ...typedBlockListContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockListSettings
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditor {
            memberGroupPicker {
              value
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditorCulture {
            memberGroupPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IMemberPickerEditor {
            memberPicker {
              members {
                properties {
                  ...typedMemberProperties
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberPickerEditorCulture {
            memberPickerCulture {
              members {
                properties {
                  ...typedMemberProperties
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditor {
            multinodeTreepicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditorCulture {
            multinodeTreepickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditor {
            multiUrlPicker {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditorCulture {
            multiUrlPickerCulture {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on INumericEditor {
            numeric {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditor {
            radiobox {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditorCulture {
            radioboxCulture {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditor {
            checkboxList {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditorCulture {
            checkboxListCulture {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditor {
            repeatableTextstrings {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditorCulture {
            repeatableTextstringsCulture {
              value
              model
              __typename
            }
          }
          ... on IRichtextEditor {
            richtext {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IRichtextEditorCulture {
            richtextCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on ISliderEditor {
            slider {
              value
              model
              __typename
            }
          }
          ... on ISliderEditorCulture {
            sliderCulture {
              value
              model
              __typename
            }
          }
          ... on ITagsEditor {
            tags {
              value
              model
              __typename
            }
          }
          ... on ITagsEditorCulture {
            tagsCulture {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditor {
            textarea {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditorCulture {
            textareaCulture {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditor {
            textstring {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditorCulture {
            textstringCulture {
              value
              model
              __typename
            }
          }
          ... on IToggleEditor {
            trueOrFalse {
              value
              model
              __typename
            }
          }
          ... on IToggleEditorCulture {
            trueOrFalseCulture {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditor {
            userPicker {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditorCulture {
            userPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditor {
            colorPicker {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditorCulture {
            colorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IContentPickerEditor {
            contentPicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IContentPickerEditorCulture {
            contentPickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on DatePickerEditor {
            datePicker {
              value
              model
              __typename
            }
            datePickerWithTime {
              value
              model
              __typename
            }
          }
          ... on DatePickerEditorCulture {
            datePickerCulture {
              value
              model
              __typename
            }
            datePickerWithTimeCulture {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditor {
            decimal {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditorCulture {
            decimalCulture {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditor {
            dropdown {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditorCulture {
            dropdownCulture {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditor {
            emailAddress {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditorCulture {
            emailAddressCulture {
              value
              model
              __typename
            }
          }
          __typename
        }
        """;

    private const string TypedImageProperties = """
        fragment typedImageProperties on TypedProperties {
          ... on IImage {
            umbracoWidth {
              value
              model
              __typename
            }
            umbracoHeight {
              value
              model
              __typename
            }
            umbracoBytes {
              value
              model
              __typename
            }
            umbracoExtension {
              value
              model
              __typename
            }
            __typename
          }
        }
        """;

    private const string TypedBlockGridContent = """
        fragment typedBlockGridContent on TypedBlockGridContentProperties {
          ... on IUmbBlockGridDemoHeadlineBlock {
            headline {
              value
              model
              __typename
            }
          }
          ... on IUmbBlockGridDemoImageBlock {
            image {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            __typename
          }
          ... on IUmbBlockGridDemoRichTextBlock {
            richText {
              value
              sourceValue
              model
              __typename
            }
            __typename
          }
        }
        """;

    private const string TypedBlockGridSettings = """
        fragment typedBlockGridSettings on TypedBlockGridSettingsProperties {
          ... on IUmbBlockGridDemoHeadlineBlock {
            headline {
              value
              model
              __typename
            }
          }
          ... on IUmbBlockGridDemoImageBlock {
            image {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            __typename
          }
          ... on IUmbBlockGridDemoRichTextBlock {
            richText {
              value
              sourceValue
              model
              __typename
            }
            __typename
          }
        }
        """;

    private const string TypedBlockListContent = """
        fragment typedBlockListContent on TypedBlockListContentProperties {
          ... on IBlockGridEditor {
            blockGrid {
              model
              gridColumns
              blocks {
                contentAlias
                settingsAlias
                rowSpan
                columnSpan
                contentProperties {
                  ...typedBlockGridContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
              }
            }
          }
          ... on IEyeDropperColorPickerEditor {
            eyeDropperColorPicker {
              value
              model
              __typename
            }
          }
          ... on IEyeDropperColorPickerEditorCulture {
            eyeDropperColorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IFileUpload {
            article {
              value
              model
              __typename
            }
            audio {
              value
              model
              __typename
            }
            file {
              value
              model
              __typename
            }
            svg {
              value
              model
              __typename
            }
            video {
              value
              model
              __typename
            }
          }
          ... on IFileUploadCulture {
            articleCulture {
              value
              model
              __typename
            }
            audioCulture {
              value
              model
              __typename
            }
            fileCulture {
              value
              model
              __typename
            }
            svgCulture {
              value
              model
              __typename
            }
            videoCulture {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditor {
            imageCropper {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditorCulture {
            imageCropperCulture {
              value
              model
              __typename
            }
          }
          ... on IMarkdownEditor {
            markdown {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMarkdownEditorCulture {
            markdownCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMediaPickerEditor {
            imageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMediaPickerEditorCulture {
            imageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditor {
            blockList {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  __typename
                }
                settingsProperties {
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditorCulture {
            blockListCulture {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  __typename
                }
                settingsProperties {
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditor {
            memberGroupPicker {
              value
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditorCulture {
            memberGroupPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IMemberPickerEditor {
            memberPicker {
              members {
                properties {
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberPickerEditorCulture {
            memberPickerCulture {
              members {
                properties {
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditor {
            multinodeTreepicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditorCulture {
            multinodeTreepickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditor {
            multiUrlPicker {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditorCulture {
            multiUrlPickerCulture {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on INumericEditor {
            numeric {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditor {
            radiobox {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditorCulture {
            radioboxCulture {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditor {
            checkboxList {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditorCulture {
            checkboxListCulture {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditor {
            repeatableTextstrings {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditorCulture {
            repeatableTextstringsCulture {
              value
              model
              __typename
            }
          }
          ... on IRichtextEditor {
            richtext {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IRichtextEditorCulture {
            richtextCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on ISliderEditor {
            slider {
              value
              model
              __typename
            }
          }
          ... on ISliderEditorCulture {
            sliderCulture {
              value
              model
              __typename
            }
          }
          ... on ITagsEditor {
            tags {
              value
              model
              __typename
            }
          }
          ... on ITagsEditorCulture {
            tagsCulture {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditor {
            textarea {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditorCulture {
            textareaCulture {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditor {
            textstring {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditorCulture {
            textstringCulture {
              value
              model
              __typename
            }
          }
          ... on IToggleEditor {
            trueOrFalse {
              value
              model
              __typename
            }
          }
          ... on IToggleEditorCulture {
            trueOrFalseCulture {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditor {
            userPicker {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditorCulture {
            userPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditor {
            colorPicker {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditorCulture {
            colorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IContentPickerEditor {
            contentPicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IContentPickerEditorCulture {
            contentPickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on DatePickerEditor {
            datePicker {
              value
              model
              __typename
            }
            datePickerWithTime {
              value
              model
              __typename
            }
          }
          ... on DatePickerEditorCulture {
            datePickerCulture {
              value
              model
              __typename
            }
            datePickerWithTimeCulture {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditor {
            decimal {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditorCulture {
            decimalCulture {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditor {
            dropdown {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditorCulture {
            dropdownCulture {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditor {
            emailAddress {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditorCulture {
            emailAddressCulture {
              value
              model
              __typename
            }
          }
          ... on IBlockListEditorCulture {
            blockListCulture {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  ...typedBlockGridContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
              }
            }
          }
          ... on ICheckboxListEditor {
            checkboxList {
              value
              model
              __typename
            }
            __typename
          }
          ... on ICheckboxListEditorCulture {
            checkboxListCulture {
              value
              model
              __typename
            }
          }
        }
        """;

    private const string TypedBlockListSettings = """
        fragment typedBlockListSettings on TypedBlockGridSettingsProperties {
          ... on IBlockGridEditor {
            blockGrid {
              model
              gridColumns
              blocks {
                contentAlias
                settingsAlias
                rowSpan
                columnSpan
                contentProperties {
                  ...typedBlockGridContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
              }
            }
          }
          ... on IEyeDropperColorPickerEditor {
            eyeDropperColorPicker {
              value
              model
              __typename
            }
          }
          ... on IEyeDropperColorPickerEditorCulture {
            eyeDropperColorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IFileUpload {
            article {
              value
              model
              __typename
            }
            audio {
              value
              model
              __typename
            }
            file {
              value
              model
              __typename
            }
            svg {
              value
              model
              __typename
            }
            video {
              value
              model
              __typename
            }
          }
          ... on IFileUploadCulture {
            articleCulture {
              value
              model
              __typename
            }
            audioCulture {
              value
              model
              __typename
            }
            fileCulture {
              value
              model
              __typename
            }
            svgCulture {
              value
              model
              __typename
            }
            videoCulture {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditor {
            imageCropper {
              value
              model
              __typename
            }
          }
          ... on IImageCropperEditorCulture {
            imageCropperCulture {
              value
              model
              __typename
            }
          }
          ... on IMarkdownEditor {
            markdown {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMarkdownEditorCulture {
            markdownCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IMediaPickerEditor {
            imageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPicker {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMediaPickerEditorCulture {
            imageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            mediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleImageMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
            multipleMediaPickerCulture {
              mediaItems {
                url(urlMode: ABSOLUTE)
                properties {
                  ...typedImageProperties
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditor {
            blockList {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  __typename
                }
                settingsProperties {
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IBlockListEditorCulture {
            blockListCulture {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  __typename
                }
                settingsProperties {
                  __typename
                }
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditor {
            memberGroupPicker {
              value
              model
              __typename
            }
          }
          ... on IMemberGroupPickerEditorCulture {
            memberGroupPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IMemberPickerEditor {
            memberPicker {
              members {
                properties {
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMemberPickerEditorCulture {
            memberPickerCulture {
              members {
                properties {
                  __typename
                }
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditor {
            multinodeTreepicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultinodeTreepickerEditorCulture {
            multinodeTreepickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditor {
            multiUrlPicker {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IMultiUrlPickerEditorCulture {
            multiUrlPickerCulture {
              links {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                target
                type
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on INumericEditor {
            numeric {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on INumericEditorCulture {
            numericCulture {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditor {
            radiobox {
              value
              model
              __typename
            }
          }
          ... on IRadioboxEditorCulture {
            radioboxCulture {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditor {
            checkboxList {
              value
              model
              __typename
            }
          }
          ... on ICheckboxListEditorCulture {
            checkboxListCulture {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditor {
            repeatableTextstrings {
              value
              model
              __typename
            }
          }
          ... on IRepeatableTextstringsEditorCulture {
            repeatableTextstringsCulture {
              value
              model
              __typename
            }
          }
          ... on IRichtextEditor {
            richtext {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on IRichtextEditorCulture {
            richtextCulture {
              value
              sourceValue
              model
              __typename
            }
          }
          ... on ISliderEditor {
            slider {
              value
              model
              __typename
            }
          }
          ... on ISliderEditorCulture {
            sliderCulture {
              value
              model
              __typename
            }
          }
          ... on ITagsEditor {
            tags {
              value
              model
              __typename
            }
          }
          ... on ITagsEditorCulture {
            tagsCulture {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditor {
            textarea {
              value
              model
              __typename
            }
          }
          ... on ITextareaEditorCulture {
            textareaCulture {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditor {
            textstring {
              value
              model
              __typename
            }
          }
          ... on ITextboxEditorCulture {
            textstringCulture {
              value
              model
              __typename
            }
          }
          ... on IToggleEditor {
            trueOrFalse {
              value
              model
              __typename
            }
          }
          ... on IToggleEditorCulture {
            trueOrFalseCulture {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditor {
            userPicker {
              value
              model
              __typename
            }
          }
          ... on IUserPickerEditorCulture {
            userPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditor {
            colorPicker {
              value
              model
              __typename
            }
          }
          ... on IColorPickerEditorCulture {
            colorPickerCulture {
              value
              model
              __typename
            }
          }
          ... on IContentPickerEditor {
            contentPicker {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on IContentPickerEditorCulture {
            contentPickerCulture {
              items {
                url(urlMode: ABSOLUTE)
                properties {
                  __typename
                }
                urlSegment
                name
                id
                key
                __typename
              }
              model
              __typename
            }
          }
          ... on DatePickerEditor {
            datePicker {
              value
              model
              __typename
            }
            datePickerWithTime {
              value
              model
              __typename
            }
          }
          ... on DatePickerEditorCulture {
            datePickerCulture {
              value
              model
              __typename
            }
            datePickerWithTimeCulture {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditor {
            decimal {
              value
              model
              __typename
            }
          }
          ... on IDecimalEditorCulture {
            decimalCulture {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditor {
            dropdown {
              value
              model
              __typename
            }
          }
          ... on IDropdownEditorCulture {
            dropdownCulture {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditor {
            emailAddress {
              value
              model
              __typename
            }
          }
          ... on IEmailAddressEditorCulture {
            emailAddressCulture {
              value
              model
              __typename
            }
          }
          ... on IBlockListEditorCulture {
            blockListCulture {
              blocks {
                contentAlias
                settingsAlias
                contentProperties {
                  ...typedBlockGridContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
              }
            }
          }
          ... on ICheckboxListEditor {
            checkboxList {
              value
              model
              __typename
            }
            __typename
          }
          ... on ICheckboxListEditorCulture {
            checkboxListCulture {
              value
              model
              __typename
            }
          }
          __typename
        }
        """;

    private const string TypedMemberProperties = """
        fragment typedMemberProperties on TypedProperties {
          ... on ITestMember {
            blockList {
              blocks {
                contentProperties {
                  ...typedBlockListContent
                  __typename
                }
                settingsProperties {
                  ...typedBlockGridSettings
                  __typename
                }
                contentAlias
                settingsAlias
                __typename
              }
              model
              __typename
            }
            umbracoMemberComments {
              value
              model
              __typename
            }
          }
          __typename
        }
        """;
}
