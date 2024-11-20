---
title: Property models reference
description: Overview of property models in Nikcio.UHeadless.
---

In Nikcio.UHeadless, property models can be extended and customized adding additional properties and functionality. The default property models provide a solid foundation for building custom property models. By extending these models, you can tailor them to your specific application requirements.

| Class Name                                     | Description                                                                                       |
|------------------------------------------------|---------------------------------------------------------------------------------------------------|
| PropertyValue                                  | A base for property values. This is the lowest level of any property value.                       |
| BlockGrid                                      | Represents a block grid property value                                                            |
| BlockList                                      | Represents a block list model                                                                     |
| ContentPicker                                  | Represents a content picker value                                                                 |
| DateTimePicker                                 | Represents a date time property value                                                             |
| DefaultProperty                                | A catch all property value that simply returns the value of the property. This is all that is needed for simple properties that doesn't need any special handling or formatting. |
| Label                                          | Gets the value of the property                                                                    |
| MediaPicker                                    | Represents a media picker item                                                                    |
| MemberPicker                                   | Represents a member picker                                                                        |
| MultiUrlPicker                                 | Represents a multi url picker                                                                     |
| NestedContent                                  | Represents nested content                                                                         |
| RichText                                       | Represents a rich text editor                                                                     |
| UnsupportedProperty                            | Represents an unsupported property value                                                          |
