namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public static class Fragments
{
    public const string PropertyValues = """
        fragment propertyValues on PropertyValue {
          ... propertyEditors
          ... blockEditor
          ... blockGridEditor

        }

        fragment blockEditor on PropertyValue {
          ... on BasicBlockListModel {
            blocks {
              contentProperties {
                value {
                  ...propertyEditors
                  ... on BasicBlockListModel {
                    blocks {
                      contentProperties {
                        value {
                          ...propertyEditors
                          ... on BasicBlockListModel {
                            blocks {
                              contentProperties {
                                value {
                                  ...propertyEditors
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
              contentAlias

              settingsAlias
            }
          }
        }

        fragment blockGridEditor on PropertyValue {
          ... on BasicBlockGridModel {
            gridColumns
            blocks {
              contentProperties {
                value {
                  ...propertyEditors
                }
              }
              settingsProperties {
                value {
                  ...propertyEditors
                }
              }
              columnSpan
              rowSpan
              areas {
                alias
                rowSpan
                columnSpan
                blocks {
                  contentProperties {
                    value {
                      ...propertyEditors
                    }
                  }
                  settingsProperties {
                    value {
                      ...propertyEditors
                    }
                  }
                  columnSpan
                  rowSpan
                  areas {
                    alias
                    rowSpan
                    columnSpan
                  }
                }
              }
            }
          }
        }

        fragment propertyEditors on PropertyValue {
          ...basicValue
          ...contentPicker
          ...dateTimePicker
          ...label
          ...mediaPicker
          ...memberPicker
          ...multiUrlPicker
          ...nestedContent
          ...richTextEditor
          ...unsupportedEditor
          model
        }

        fragment nestedContentPropertyEditors on PropertyValue {
          ...basicValue
          ...contentPicker
          ...dateTimePicker
          ...label
          ...mediaPicker
          ...slimMemberPicker
          ...multiUrlPicker
          ...richTextEditor
          ...unsupportedEditor
          model
        }

        fragment memberPropertyEditors on PropertyValue {
          ...basicValue
          ...contentPicker
          ...dateTimePicker
          ...label
          ...mediaPicker
          ...slimMemberPicker
          ...multiUrlPicker
          ...nestedContent
          ...richTextEditor
          ...unsupportedEditor
          model
        }

        fragment basicValue on PropertyValue {
          ... on BasicPropertyValue {
            basicValue: value
          }
        }

        fragment contentPicker on PropertyValue {
          ... on BasicContentPicker {
            contentList {
              id
              absoluteUrl
              key
              name
              url
              urlSegment
            }
          }
        }

        fragment dateTimePicker on PropertyValue {
          ... on BasicDateTimePicker {
            dateTime: value
          }
        }

        fragment label on PropertyValue {
          ... on BasicLabel {
            label: value
          }
        }

        fragment mediaPicker on PropertyValue {
          ... on BasicMediaPicker {
            mediaItems {
              id
              url
            }
          }
        }

        fragment memberPicker on PropertyValue {
          ... on BasicMemberPicker {
            members {
              id
              name
              properties {
                alias
                value {
                  ...memberPropertyEditors
                }
                editorAlias
              }
            }
          }
        }

        fragment slimMemberPicker on PropertyValue {
          ... on BasicMemberPicker {
            members {
              id
              name
            }
          }
        }

        fragment multiUrlPicker on PropertyValue {
          ... on BasicMultiUrlPicker {
            links {
              name
              target
              type
              url
            }
          }
        }

        fragment nestedContent on PropertyValue {
          ... on BasicNestedContent {
            elements {
              properties {
                alias
                value {
                  ...nestedContentPropertyEditors
                }
                editorAlias
              }
            }
          }
        }

        fragment richTextEditor on PropertyValue {
          ... on BasicRichText {
            richText: value
            sourceValue
          }
        }

        fragment unsupportedEditor on PropertyValue {
          ... on BasicUnsupportedPropertyValue {
            message
          }
        }
        
        """;
}
