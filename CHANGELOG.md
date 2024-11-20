# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [6.0.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v5.1.0...v6.0.0) (2024-11-20)


### Features

* Update docs site ([274b67b](https://github.com/nikcio/Nikcio.UHeadless/commit/274b67b9c763642b9a49be6190102f0741560447))
* Update v6 ([f489f72](https://github.com/nikcio/Nikcio.UHeadless/commit/f489f720b1637c5a42a6424bd5659894cc8680e3))

## [6.0.0-preview001](https://github.com/nikcio/Nikcio.UHeadless/compare/v5.0.0-preview005...v6.0.0-preview001) (2024-08-29)


### Features

* Build on Umbraco v14 ([ce2b75e](https://github.com/nikcio/Nikcio.UHeadless/commit/ce2b75ed36094e45f11fe9b83a5b7fd2d122eb99))
* Update test projects ([5e2f62e](https://github.com/nikcio/Nikcio.UHeadless/commit/5e2f62e4c53de886134dc22521af37950c111e39))

## [5.1.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v5.0.0...v5.1.0) (2024-11-20)


### Features

* Centrialize package versions ([b21215f](https://github.com/nikcio/Nikcio.UHeadless/commit/b21215fe8d7d43944e0c565cc43cdbeef5694c55))
* Update dependencies ([4fdd319](https://github.com/nikcio/Nikcio.UHeadless/commit/4fdd31993253a7fdfb3a414af6801d4e4b178848))

## [5.0.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.2.1...v5.0.0) (2024-6-26)

## ✨ Highlights

- Simpler project structure
  - You now only need to reference the `Nikcio.UHeadless` package to get started as all packages are now merged into one.
- New defaults for Queries and Models
  - The new defaults are now simpler and limited to the most common useable queries and models.
- Authorization by default
  - New defaults will include authorization by default which will help ensure you're not exposing too much data.
- Easier to use Typed properties
  - This release brings typed or previously 'Named properties' to the forefront and makes them easier to use.
  - Typed properties are now available everywhere it makes sense. (See the feature section for more details)
- Cleaner setup with new bootstrapping.
  - The new bootstrapping is simpler and more intuitive.

## 🐎 Get started
[Getting started with version 5.0.0](https://nikcio.github.io/Nikcio.UHeadless/v5/fundamentals/getting-started/)

## 🚀 Features
- Typed properties are now available everywhere it makes sense.
  - Use the `TypedPropertes` class for Content items/Media items and Member items
  - Use the `TypedNestedContentProperties` class for Nested Content items
  - Use the `TypedBlockListContentProperties` class for Block List items (Content)
  - Use the `TypedBlockListSettingsProperties` class for Block List items (Settings)
  - Use the `TypedBlockGridContentProperties` class for Block Grid items (Content)
  - Use the `TypedBlockGridSettingsProperties` class for Block Grid items (Settings)
- New defaults for Queries and Models
  - Queries
    - Use `ContentAtRootQuery` to get content at the root of your Umbraco site.
    - Use `ContentByContentTypeQuery` to get content by content type.
    - Use `ContentByGuidQuery` to get content by guid.
    - Use `ContentByIdQuery` to get content by id.
    - Use `ContentByRouteQuery` to get content by route.
      - Includes built-in support for the Umbraco automatic redirects.
    - Use `ContentByTagQuery` to get content by tag.
    - Use `MediaAtRootQuery` to get media at the root of your Umbraco site.
    - Use `MediaByContentTypeQuery` to get media by content type.
    - Use `MediaByGuidQuery` to get media by guid.
    - Use `MediaByIdQuery` to get media by id.
    - Use `FindMembersByDisplayNameQuery` to get members by thier display name.
    - Use `FindMembersByEmailQuery` to get members by email.
    - Use `FindMembersByRoleQuery` to get members by role.
    - Use `FindMembersByUsernameQuery` to get members by username.
    - Use `MemberByEmailQuery` to get a member by email.
    - Use `MemberByGuidQuery` to get a member by guid.
    - Use `MemberByIdQuery` to get a member by id.
    - Use `MemberByUsernameQuery` to get a member by username.
  - Models
    - `ContentItem` is used for Content queries. This has built-in support for redirect information provided by `ContentByRouteQuery`.
    - `MediaItem` is used for Media queries.
    - `MemberItem` is used for Member queries.
    - `BlockGrid` to support the block grid editor.
    - `BlockList` to support the block list editor.
    - `ContentPicker` to support the content picker editor and the multi node picker.
    - `DateTimePicker` to support the date time picker editor.
    - `DefaultProperty` to support editors that doesn't require special modeling. (Used as the fallback)
    - `Label` to support the label editor.
    - `MediaPicker` to support the media picker editor.
    - `MemberPicker` to support the member picker editor. (Now requires Authorization by default to avoid leaking member information. This can be disabled by setting `options.DisableAuthorization`)
    - `MultiUrlPicker` to support the multi url picker editor.
    - `NestedContent` to support the nested content editor.
    - `RichText` to support the rich text editor and the markdown editor.
    - `UnsupportedProperty` to mark properties that are not supported.
- Authorization by default
  - Authorization is now enabled by default for all queries to have secure by default queries.
  - Authorization is now enabled by default for the `MemberPicker` model to avoid leaking member information.
  - This can be disabled by setting `options.DisableAuthorization` in the `UmbracoHeadlessOptions`.
- New bootstrapping
  - Bootstrapping is now simpler and more intuitive. (See the getting started guide at the bottom of this release note)
- The new `ContentPicker` model now supports querying the properties of the picked content.
- The `IResolverContext` from HotChocolate has now been added to the default commands so it can be used to resolve services and access query arguments like `Culture`, `IncludePreivew`, `Fallback` and `Segment`.
- `ContentByContentTypeQuery` now supports `includePreview` to fetch preview content. #229
- Context data like `Culture`, `IncludePreivew`, `Fallback` and `Segment` are now provided with the `inContext` parameter object on content queries.
- Instead of the `UsePaging` attribute from HotChocolate a new `Paginationresult` model has been added to avoid problems with the way Umbraco handles content data on queries. This means that the `UsePaging` attribute is no longer needed and shouldn't be used in custom queries either.

## 📖 Documentation Updates
- This release brings an overhaul of the documentation bringing you the most helpful information about the package.

## 🧪 Test improvements
- Content query tests have been rewritten and are now more stable.
- Media query tests have been rewritten and are now more stable.
- Member query tests have been rewritten and are now more stable.
- Code coverage is now over 80%.

## 🏛️Project stability
- All projects are now merged into one which makes maintenance easier as this also removes a lot of code as the package could be simplified.
- A new Nikcio.UHeadless.LegacyModels project has been added to help with migration from the old schema to the new schema. (This won't be available as a package but will be available for use for people migrating).
  - Use this project to get a similar content model to the one found in v4.

## 📦 Dependencies
- `HotChocolate.Data` has been removed as it's no longer needed.

## 💥 Breaking changes.
- This release is a complete overhaul of the project and therefore a lot of things have changed.
  - Take a good look in the documentation which has been updated and if not your question can be answered there open a question on GitHub.

## [4.2.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.2.0...v4.2.1) (2024-04-09)


### Features

* Add analyzers and correct warnings ([9503de8](https://github.com/nikcio/Nikcio.UHeadless/commit/9503de8fe6f30bffd84335f257b0242bc89f3dce))
* Align ArgumentNullException ([0cf4fc2](https://github.com/nikcio/Nikcio.UHeadless/commit/0cf4fc2df4abc39ad9e3f95eae8f348db2ff9dd1))
* Minor clean up ([cc80dfb](https://github.com/nikcio/Nikcio.UHeadless/commit/cc80dfbb09c0dd4017b612f4fa30ea7e392c5f46))


### Bug Fixes

* Empty string being returned as array ([d8cc412](https://github.com/nikcio/Nikcio.UHeadless/commit/d8cc412540267895f1b01504995df7d99a8b6a5b))

## [4.2.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.2.0-preview002...v4.2.0) (2024-04-06)


### Features

* Convert unit test project ([457ed6a](https://github.com/nikcio/Nikcio.UHeadless/commit/457ed6a50bd7f30f097370f1232b57ae5ed19181))
* Doc deps ([395a6b8](https://github.com/nikcio/Nikcio.UHeadless/commit/395a6b83224c07425c6b5f5f15545033e2bf3747))
* Fix timezone diff ([83d6c0a](https://github.com/nikcio/Nikcio.UHeadless/commit/83d6c0a360faa58b8676b284083b75a72e2edf71))
* Format date times ([07c2d57](https://github.com/nikcio/Nikcio.UHeadless/commit/07c2d57963a0617ea3f687ccf99a82d8044250f2))
* Install sharp to docs project ([c937e64](https://github.com/nikcio/Nikcio.UHeadless/commit/c937e646d5c8c562b268a884f116fedccc1da42a))
* Recreate snapshots ([31a38dd](https://github.com/nikcio/Nikcio.UHeadless/commit/31a38dd8dde92dc09cc65e02c24e78bb1f5168f9))
* Reset snapshots ([3955f84](https://github.com/nikcio/Nikcio.UHeadless/commit/3955f84fb84ada7a91336d5b48be7ff155508e3e))
* Rewrite tests ([a0deda4](https://github.com/nikcio/Nikcio.UHeadless/commit/a0deda4cf65b80bf8c6ae6665dad9a18ace67782))
* Update deps ([78ea1a5](https://github.com/nikcio/Nikcio.UHeadless/commit/78ea1a593322f9a13414def9cb7c449f984dfffa))
* Update snapshots ([623f77a](https://github.com/nikcio/Nikcio.UHeadless/commit/623f77af27f5bdb7a6201f27aa009d834306cf9e))
* Update test deps ([6e4d95d](https://github.com/nikcio/Nikcio.UHeadless/commit/6e4d95de4246a4f3528e5788f487af01de8fab3e))


### Bug Fixes

* Correctly escape backslash ([f3f6a23](https://github.com/nikcio/Nikcio.UHeadless/commit/f3f6a23877bd72a2c6427d15699c2ab854b80738))
* Fixed path for snapshots ([8ee1239](https://github.com/nikcio/Nikcio.UHeadless/commit/8ee1239ee63f7c86b2035f3189f551e829db777a))

## [4.2.0-preview001](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.1.1...v4.2.0-preview001) (2024-03-06)


### Features

* update deps 3 ([efc9c6c](https://github.com/nikcio/Nikcio.UHeadless/commit/efc9c6c7ae429aeed3217184071c4809111ba730))
* Update usync files ([b1e3d9c](https://github.com/nikcio/Nikcio.UHeadless/commit/b1e3d9c00869bc5c7293695d6f760392735eb494))

### [4.1.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.1.0...v4.1.1) (2023-08-02)


### Bug Fixes

* Use internal composers ([182e084](https://github.com/nikcio/Nikcio.UHeadless/commit/182e084b267b0f9f86eda359467199fe58951689))

## [4.1.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.0.0...v4.1.0) (2023-07-02)


### Features

* Added the abillity to query properties based on content types in Umbraco ([f93a030](https://github.com/nikcio/Nikcio.UHeadless/commit/f93a03091e1a378244cb0be4d195abb9879cbe6d))
* Added type modules to media & members ([c5f8711](https://github.com/nikcio/Nikcio.UHeadless/commit/c5f87111e0626c2531825fac7a5206aebe363a0d))


### Bug Fixes

* Fixed DependencyReflectorFactory using too many of the required params ([006d721](https://github.com/nikcio/Nikcio.UHeadless/commit/006d721f33f45860ce355697adc1d9dbd2a99b8f))
* Only add used types to PropertyMap if mapping was added ([6f7d43c](https://github.com/nikcio/Nikcio.UHeadless/commit/6f7d43ca16d8216b556dc79d335d16250dc49f57))

## [4.0.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.0.0-preview002...v4.0.0) (2023-06-12)


### ⚠ BREAKING CHANGES

* Removes the redundant `TProperty` argument from classes where it's not needed which simplifies the classes quite a bit.

### Features

* Added mapping key methods to property map ([36bd25e](https://github.com/nikcio/Nikcio.UHeadless/commit/36bd25e10f4a2cae8aad8df7e182698f39c43d2d))

## [4.0.0-preview002](https://github.com/nikcio/Nikcio.UHeadless/compare/v4.0.0-preview001...v4.0.0-preview002) (2023-06-07)


### ⚠ BREAKING CHANGES

* Removes the `UseSecurity` option. The naming is confusing on what it does and the developer should control the authentication and authorization themself to have greater control over their application.

### Features

* Allow `BasicMember` with generic properties ([abf399d](https://github.com/nikcio/Nikcio.UHeadless/commit/abf399dcbb8508ef27c5f526e0cbf134961d43a1))


* Removes the `UseSecurity` option ([530870a](https://github.com/nikcio/Nikcio.UHeadless/commit/530870a1fec4613eb11cd6b3b9c637f50aad60cf))

## [4.0.0-preview001](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.3.0...v4.0.0-preview001) (2023-06-06)


### ⚠ BREAKING CHANGES

* Renamed `AllMembers` query to `MembersAll` to have similar naming as the rest of the queries.
* Some namespaces wasn't synced properly to the location of the files. So to avoid confusion over source files the namespaces has been synced. New namespaces:

BasicBlockListItem - `Nikcio.UHeadless.Base.Basics.EditorsValues.BlockList.Models`
BasicBlockListModel - `Nikcio.UHeadless.Base.Basics.EditorsValues.BlockList.Models`
BasicContentPicker - `Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models`
BasicContentPickerItem - `Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models`
BasicDateTimePicker - `Nikcio.UHeadless.Base.Basics.EditorsValues.DateTimePicker.Models`
BasicPropertyValue - `Nikcio.UHeadless.Base.Basics.EditorsValues.Fallback.Models`
BasicMediaPicker - `Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models`
BasicMediaPickerItem - `Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models`
BasicMemberPicker - `Nikcio.UHeadless.Base.Basics.EditorsValues.MemberPicker.Models`
BasicMemberPickerItem - `Nikcio.UHeadless.Base.Basics.EditorsValues.MemberPicker.Models`
BasicMultiUrlPicker - `Nikcio.UHeadless.Base.Basics.EditorsValues.MultiUrlPicker.Models`
BasicMultiUrlPickerItem - `Nikcio.UHeadless.Base.Basics.EditorsValues.MultiUrlPicker.Models`
BasicNestedContent - `Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models`
BasicNestedContentElement - `Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models`
BasicRichText - `Nikcio.UHeadless.Base.Basics.EditorsValues.RichTextEditor.Models`
PropertyMapExtensions - `Nikcio.UHeadless.Base.Basics.Maps.Extensions`
BasicProperty - `Nikcio.UHeadless.Base.Basics.Models`
* Member queries have been split to separate models which removes `BasicMemberQuery`.
* Media queries have been split to separate models which removes `BasicMediaQuery`.
* This removes the property queries as they had a weird place in the package and no real use. (You can do the exact same thing with the content queries).
* Media & Members have had culture removed from the query options. It's not possible to create media and members on different cultures and it's therefore not necessary to be able to query for it.

Content & Property queries have had `segment` & `Fallback` added to the query options to better support culture querying.

All `GetValue` for property values have been changed to `Value` which better support culture variants.
* Introduces `IVariationContextAccessor` to the contructor on `BasicContent`
* What was `BasicContentOfBasicPropertyAndBasicContentTypeAndBasicContentRedirect` in v3 schema will now be `BasicContent`. This simplifies the naming of types used in the schema a lot.

Also the integration test schema was updated using `dotnet graphql download https://localhost:44321/graphql`
* The content queries has been given a separate class for each query to help developers only expose the data they need.
* Replace Alias with model on propertyValue
* **deps:** Added min requirement to be Umbraco 11 & .Net 7
* **deps:** Updated to Hotchocolate 13

### Features

* Added Auth queries ([f93f598](https://github.com/nikcio/Nikcio.UHeadless/commit/f93f59888e947df143a780fe23be72d40e2a6606))
* Added better support for multi-culture sites ([fb0be99](https://github.com/nikcio/Nikcio.UHeadless/commit/fb0be99ba693203060e24692db1eb52348111ff3))
* Added Block grid support ([c92e40c](https://github.com/nikcio/Nikcio.UHeadless/commit/c92e40c982b7988214204c4eaf2814630cf44926))
* Replace Alias with model on propertyValue ([406c102](https://github.com/nikcio/Nikcio.UHeadless/commit/406c102712d292012176580ad25e8bc33a2a894a))
* Split media queries to separate models ([368072f](https://github.com/nikcio/Nikcio.UHeadless/commit/368072fd1a71464072029649c655314950308ae6))
* Split member queries to separate models ([469e7df](https://github.com/nikcio/Nikcio.UHeadless/commit/469e7dfc6440da63c81df44daf4e654189269479))


### Bug Fixes

* Fixes culture properties on content ([aee9e0b](https://github.com/nikcio/Nikcio.UHeadless/commit/aee9e0b76a763db3b64e80f070d121abfd63bd58)), closes [#145](https://github.com/nikcio/Nikcio.UHeadless/issues/145)


* Changed BasicContent to have simpler names in schema ([c8c8eea](https://github.com/nikcio/Nikcio.UHeadless/commit/c8c8eeaf485531053feeec67077a1ef455e686e4))
* Conform namespaces after folder structure ([611184c](https://github.com/nikcio/Nikcio.UHeadless/commit/611184cca6449d7bf493f5d8c3ff8a4bc47c0d42))
* **deps:** Added min requirement to be Umbraco 11 & .Net 7 ([7683ebb](https://github.com/nikcio/Nikcio.UHeadless/commit/7683ebbc7c1cf1570bdcbe72947428207eb9d189))
* **deps:** Updated to Hotchocolate 13 ([91bb540](https://github.com/nikcio/Nikcio.UHeadless/commit/91bb540e405601ffa14cb5151c88b5bce621004e))
* Remove property queries ([a9565e0](https://github.com/nikcio/Nikcio.UHeadless/commit/a9565e0a7354b03de72cf062d1f4481667a6d8ad))
* Rename `AllMembers` to `MembersAll` to conform to other query names ([f441c87](https://github.com/nikcio/Nikcio.UHeadless/commit/f441c878767929bf7b8f816218facfd56e107ea3))
* Split content queries into separate files ([09e1e66](https://github.com/nikcio/Nikcio.UHeadless/commit/09e1e66415e60fbbee748dd7c760a34460e2710f))

### [3.3.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.2.0...v3.2.1) (2023-01-18)

### Features

* Support query for HTML output from MarkdownEditor ([PR 110](https://github.com/nikcio/Nikcio.UHeadless/pull/110)) - Thanks @thetanz-geoff

### Bug Fixes

* Fixed possible null ref ([7e8efd4](https://github.com/nikcio/Nikcio.UHeadless/commit/7e8efd45ace10db403f63ea7af567ceb296e6995))

### Dependencies

* Updated dependencies
  * Hotchocolate updated to 12.16.0

## [3.2.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.1.0...v3.2.0) (2022-11-22)


### Features

* Add option to set GraphQLServerOptions ([bf31138](https://github.com/nikcio/Nikcio.UHeadless/commit/bf31138f6416c9e03d091d47ed42d8277ed19b1a))

## [3.1.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.6...v3.1.0) (2022-11-16)


### Features

* New content queries ([8a28fc7](https://github.com/nikcio/Nikcio.UHeadless/commit/8a28fc74260cbee15adec2def7c25528d03bb4d0))

  - ContentAll (Gets all the content items available)
  - ContentDescendantsByGuid (Gets descendants on a content item selected by guid)
  - ContentDescendantsById (Gets descendants on a content item selected by id)
  - ContentDescendantsByContentType (Gets all descendants of content items with a specific content type)
  - ContentDescendantsByAbsoluteRoute (Gets content item descendants by an absolute route)
  - ContentByTag (Gets content items by tag)

### [3.0.6](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.5...v3.0.6) (2022-11-03)


### Bug Fixes

* Fixed redirect encountering "Ambitious reference" ([5bf1ea4](https://github.com/nikcio/Nikcio.UHeadless/commit/5bf1ea4c6470a17277eb0026baa615bc3adb5846))

### Dependencies

* Updated dependencies
  * Hotchocolate updated to 12.15.1

### [3.0.5](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.4...v3.0.5) (2022-10-18)


### Bug Fixes

* Create unsupported message for grid ([a3f4798](https://github.com/nikcio/Nikcio.UHeadless/commit/a3f479838db7f78946aafb36558a73aae01e5b7a))

### Dependencies

* Updated dependencies
  * Hotchocolate updated to 12.15.0

### [3.0.4](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.3...v3.0.4) (2022-10-05)


### Bug Fixes

* Fixed default mapping overruling custom property mappings ([fd89095](https://github.com/nikcio/Nikcio.UHeadless/commit/fd89095cd78d87ca86cd1a96757a3195dea2c8fe))

### [3.0.3](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.2...v3.0.3) (2022-10-05)


### Bug Fixes

* Add culture to basic block list getValue ([04db15f](https://github.com/nikcio/Nikcio.UHeadless/commit/04db15f802b17da686b19c3f6c0e919c58a96c9e)), closes [#91](https://github.com/nikcio/Nikcio.UHeadless/issues/91)

### Dependencies

* Updated dependencies

## [3.0.2](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.1...v3.0.2) (2022-09-19)

### Bug fixes

* Fixed minimum umbraco cms requirement. (Minimum requirement is now set to v10.0.0)

### Dependencies

* Updated dependencies

## [3.0.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.0...v3.0.1) (2022-09-03)

### Dependencies

* Updated dependencies

## [3.0.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.0-preview003...v3.0.0) (2022-08-03)

## [3.0.0-preview003](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.0-preview002...v3.0.0-preview003) (2022-08-03)


### ⚠ BREAKING CHANGES

* Namespaces of Basics have changed like so:
Nikcio.UHeadless.Basics.Properties -->
Nikcio.UHeadless.Properties.Basics

### Features

* Added manifest to composer ([7ebfd9d](https://github.com/nikcio/Nikcio.UHeadless/commit/7ebfd9dd3d352f4cb6366505afaba3e7242ff13f))
* Added member queries & BasicLabel model ([#68](https://github.com/nikcio/Nikcio.UHeadless/issues/68)) ([4772257](https://github.com/nikcio/Nikcio.UHeadless/commit/47722573b9f2d029051b0f449df95b9ba9a39810))
* Added more member queries ([305fc80](https://github.com/nikcio/Nikcio.UHeadless/commit/305fc80fee252fd75dd4b3498f64b96b5cb2a8f3))


* Merged basic projects with the base projects ([4e90383](https://github.com/nikcio/Nikcio.UHeadless/commit/4e903836ffc9fda0b54d162e301d41776dfb1bb0))

## [3.0.0-preview002](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.0-preview001...v3.0.0-preview002) (2022-07-16)


### Bug Fixes

* Fixed basics project references ([ac5c961](https://github.com/nikcio/Nikcio.UHeadless/commit/ac5c961c35ddff4ecfbc66a35c4eefa45626eac3))

## [3.0.0-preview001](https://github.com/nikcio/Nikcio.UHeadless/compare/v3.0.0-preview000...v3.0.0-preview001) (2022-07-16)

### BREAKING CHANGES

* Dropped support for Umbraco v9

* Namespace changes
  * Many namespaces have been changed to make it easier to make extending packages. This also means that some classes have been moved to completely different namespaces.
  * If you need to find a class's new location, look in the docs or use the search feature on GitHub.
  * Example:
    * `using Nikcio.UHeadless.UmbracoContent` --> `using Nikcio.UHeadless.Content`

* Querying properties have been changed. See [How to query properties](docs/v3/querying/properties.md).

* The `PropertyValue` which is a basis for all property values now always includes an Alias. This is to support the new Property querying with fragments. See [How to query properties](docs/v3/querying/properties.md).

* `AddPropertyMapDefaults` has been removed from `IPropertyMap` this now

* `GetProperties` has been changed to `GetContentItemsProperties` on the `IPropertyRespository`

* The `Value` property on `BasicProperty` has been changed from `object` to `PropertyValue` to support the new querying. See [How to query properties](docs/v3/querying/properties.md).

* `ContentQuery` now takes a new generic parameter `TContentRedirect` of type `IContentRedirect` for redirect information.

* `GetContentByRoute` has been removed. Use `GetContentByAbsoluteRoute` instead.

### Features

* Added boilerplate for members ([1dd04b7](https://github.com/nikcio/Nikcio.UHeadless/commit/1dd04b7e3e1f1bdea3751633d8c10e6e39096e2b))
* Added ContentRedirect variation to BasicContent ([c9d7912](https://github.com/nikcio/Nikcio.UHeadless/commit/c9d7912da56aac41d03c577c2355be6de9060b31))
* Added ContentRouter ([52cc95a](https://github.com/nikcio/Nikcio.UHeadless/commit/52cc95a39a99b12d2ed35be0736ba2a7f9e9c063))
* Added GetMediaByContentType ([c725dbf](https://github.com/nikcio/Nikcio.UHeadless/commit/c725dbf9e86d994df5f9de756e390b49f3f2d766))
* Added ICommand ([388bd05](https://github.com/nikcio/Nikcio.UHeadless/commit/388bd05f66d04a02e15853179c6fa51c2e64ac54))
* Added non generic BasicBlockListItem ([8aa14b9](https://github.com/nikcio/Nikcio.UHeadless/commit/8aa14b9c8a18d158efdb312ab04041042fde8888))
* Added non generic BasicBlockListModel ([6cb3b06](https://github.com/nikcio/Nikcio.UHeadless/commit/6cb3b0670c10c55d5c36ee86f8a6647bc0e131de))
* Added non generic BasicMemberPicker ([a8086e0](https://github.com/nikcio/Nikcio.UHeadless/commit/a8086e059e94d3aaca1ae59f5fd61e2b5d314d54))
* Added non generic BasicMemberPickerItem ([989650b](https://github.com/nikcio/Nikcio.UHeadless/commit/989650bf22732969f163d5da419a727f77c56c78))
* Added TMedia to BasicMedia ([07481e2](https://github.com/nikcio/Nikcio.UHeadless/commit/07481e233d05e17da432761d12215eaa596e7a32))
* Added type support for properties ([19cedce](https://github.com/nikcio/Nikcio.UHeadless/commit/19cedceb8ec6308ae441d08418fe3f10c3d739ce))
* Adds non generic BasicNestedContent ([da2f6db](https://github.com/nikcio/Nikcio.UHeadless/commit/da2f6dbf0cd8d20a5e958be5ae56f67e6b0fb3a3))
* Adds non generic BasicNestedContentElement ([8a1d37e](https://github.com/nikcio/Nikcio.UHeadless/commit/8a1d37e998b8530b4177ebb77d258b3a5b50755d))
* Refactored Content and Media repository ([e0d1723](https://github.com/nikcio/Nikcio.UHeadless/commit/e0d1723af7df4c55748f5b784ef1d565f6391838))


### Bug Fixes

* Added missing logger argument ([7db3158](https://github.com/nikcio/Nikcio.UHeadless/commit/7db3158addf555511bfd11eac097b6880edfa222))

## [3.0.0-preview000](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.3.0...v3.0.0-preview000) (2022-07-16)

This release is to test the release workflow.

### BREAKING CHANGES
This is work in progress and a summary of the breaking changes will come.

### Features

* Added boilerplate for members ([1dd04b7](https://github.com/nikcio/Nikcio.UHeadless/commit/1dd04b7e3e1f1bdea3751633d8c10e6e39096e2b))
* Added ContentRedirect variation to BasicContent ([c9d7912](https://github.com/nikcio/Nikcio.UHeadless/commit/c9d7912da56aac41d03c577c2355be6de9060b31))
* Added ContentRouter ([52cc95a](https://github.com/nikcio/Nikcio.UHeadless/commit/52cc95a39a99b12d2ed35be0736ba2a7f9e9c063))
* Added GetMediaByContentType ([c725dbf](https://github.com/nikcio/Nikcio.UHeadless/commit/c725dbf9e86d994df5f9de756e390b49f3f2d766))
* Added ICommand ([388bd05](https://github.com/nikcio/Nikcio.UHeadless/commit/388bd05f66d04a02e15853179c6fa51c2e64ac54))
* Added non generic BasicBlockListItem ([8aa14b9](https://github.com/nikcio/Nikcio.UHeadless/commit/8aa14b9c8a18d158efdb312ab04041042fde8888))
* Added non generic BasicBlockListModel ([6cb3b06](https://github.com/nikcio/Nikcio.UHeadless/commit/6cb3b0670c10c55d5c36ee86f8a6647bc0e131de))
* Added non generic BasicMemberPicker ([a8086e0](https://github.com/nikcio/Nikcio.UHeadless/commit/a8086e059e94d3aaca1ae59f5fd61e2b5d314d54))
* Added non generic BasicMemberPickerItem ([989650b](https://github.com/nikcio/Nikcio.UHeadless/commit/989650bf22732969f163d5da419a727f77c56c78))
* Added TMedia to BasicMedia ([07481e2](https://github.com/nikcio/Nikcio.UHeadless/commit/07481e233d05e17da432761d12215eaa596e7a32))
* Added type support for properties ([19cedce](https://github.com/nikcio/Nikcio.UHeadless/commit/19cedceb8ec6308ae441d08418fe3f10c3d739ce))
* Adds non generic BasicNestedContent ([da2f6db](https://github.com/nikcio/Nikcio.UHeadless/commit/da2f6dbf0cd8d20a5e958be5ae56f67e6b0fb3a3))
* Adds non generic BasicNestedContentElement ([8a1d37e](https://github.com/nikcio/Nikcio.UHeadless/commit/8a1d37e998b8530b4177ebb77d258b3a5b50755d))
* Refactored Content and Media repository ([e0d1723](https://github.com/nikcio/Nikcio.UHeadless/commit/e0d1723af7df4c55748f5b784ef1d565f6391838))


### Bug Fixes

* Added missing logger argument ([7db3158](https://github.com/nikcio/Nikcio.UHeadless/commit/7db3158addf555511bfd11eac097b6880edfa222))

## [2.3.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.2.1...v2.3.0) (2022-07-06)


### Features

* Added package telemetry data ([5c6a4b0](https://github.com/nikcio/Nikcio.UHeadless/commit/5c6a4b01a6ee14644a11cf0ee15d79947c575d53))

### Dependencies

* Updated dependencies

### [2.2.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.2.0...v2.2.1) (2022-06-19)

### Dependencies

* Updated dependencies

### Bug fixes

* Allow ParameterType to be IsAssignableFrom() ([#57](https://github.com/nikcio/Nikcio.UHeadless/issues/57)) ([221b13a](https://github.com/nikcio/Nikcio.UHeadless/commit/221b13a7803ed7e213e9831bac415c32081290ce))

## [2.2.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.1.1...v2.2.0) (2022-06-06)


### Features

* Added DateTime picker model ([bdfa211](https://github.com/nikcio/Nikcio.UHeadless/commit/bdfa21149114b1a23b7d702b1b97ee86e06628b7))
* **deps:** Updated dependencies ([5020a3e](https://github.com/nikcio/Nikcio.UHeadless/commit/5020a3e68b5b8d0b4c737fce9b228cf570101e95))

## [2.1.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.1.0...v2.1.1) (2022-05-22)


### Dependencies

* Updated dependencies

## [2.1.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.2...v2.1.0) (2022-05-10)


### Features

* Added GetContentByAbsoluteRoute ([208f2dd](https://github.com/nikcio/Nikcio.UHeadless/commit/208f2dd7863e7610b782d4d0e40e68b97c159b57))

### [2.0.2](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.1...v2.0.2) (2022-05-09)


### Bug Fixes

* Fixed BasicMultiUrlPicker not getting value for single link ([#48](https://github.com/nikcio/Nikcio.UHeadless/issues/48)) ([391d1ee](https://github.com/nikcio/Nikcio.UHeadless/commit/391d1eee502ceb33931ae3167f23f2a1f4ed21d2))

## [2.0.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.0...v2.0.1) (2022-05-04)


### Dependencies

* Updated dependencies

## [2.0.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.0-preview.3...v2.0.0) (2022-05-01)


### Features

* Add id to media picker item ([#43](https://github.com/nikcio/Nikcio.UHeadless/issues/43)) ([eb9e40d](https://github.com/nikcio/Nikcio.UHeadless/commit/eb9e40d184aeb69bfe14dd01560c0d606209445f))

## [2.0.0-preview.3](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.0-preview.2...v2.0.0-preview.3) (2022-04-26)


### Bug Fixes

* Added correct alias to block list ([fa026bf](https://github.com/nikcio/Nikcio.UHeadless/commit/fa026bf92bc55068d1a01052f8f72fc4c4f7803c))

## [2.0.0-preview.2](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.0-preview.1...v2.0.0-preview.2) (2022-04-26)


### ⚠ BREAKING CHANGES

* BasicMediaItem --> BasicMediaPickerItem
MediaItem -- MeidaPickerItem
BasicMemberItem --> BasicMemberPickerItem
MemberItem --> MemberPickerItem
BasicLinkItem --> BasicMultiUrlPickerItem
LinkItem --> MultiUrlPickerItem
* BasicMember --> BasicMemberItem
CreteMember --> CreateMemberPickerItem
Member --> MemberItem
CreateLink --> CreateLinkPickerItem
BasicLink ..> BasicLinkItem
Link --> LinkItem

### Features

* Added editor alias to BasicBlockListModel ([da7c885](https://github.com/nikcio/Nikcio.UHeadless/commit/da7c885943bccc27f78ae37d68ee1c24728d5398))
* Added generic media picker ([811295a](https://github.com/nikcio/Nikcio.UHeadless/commit/811295a56a99930f0fdc9375dc420640a5951245))
* Added generic member picker ([c6597a3](https://github.com/nikcio/Nikcio.UHeadless/commit/c6597a38b4c18bac81828687c49fcea7722ff1cd))
* Added generic models for multi url picker ([befc3ab](https://github.com/nikcio/Nikcio.UHeadless/commit/befc3abc0b82884b698c1a426eb5468cc6b7affa))
* Added generic models to content picker ([4e1fa3b](https://github.com/nikcio/Nikcio.UHeadless/commit/4e1fa3b02bc50c9d4a49614d58dad0d4c03f20f7))


### Bug Fixes

* Fixed creation of link item ([55b3052](https://github.com/nikcio/Nikcio.UHeadless/commit/55b3052c3173062c385314edca0469f0ad68f7cb))
* Made methods virtual ([d558c18](https://github.com/nikcio/Nikcio.UHeadless/commit/d558c18c1b1de5d8984f543dbdde656e3b52874a))


* Changed naming of picker models to have similar naming ([3dbbc43](https://github.com/nikcio/Nikcio.UHeadless/commit/3dbbc43f8c656adda4553f15d0f390f6ced92bee))
* Changed picker model naming ([a9727d9](https://github.com/nikcio/Nikcio.UHeadless/commit/a9727d92d08537ccdb4f3aac2a34a919548b3ea6))

## [2.0.0-preview.1](https://github.com/nikcio/Nikcio.UHeadless/compare/v2.0.0-preview.0...v2.0.0-preview.1) (2022-04-24)


### Features

* Add query by content type and filters to children ([#36](https://github.com/nikcio/Nikcio.UHeadless/issues/36)) ([9467bc5](https://github.com/nikcio/Nikcio.UHeadless/commit/9467bc54653d86dbb687e00af9bfcc5e12310d07)) Thanks @Rizzet

### Dependencies

* Updated dependencies

## [2.0.0-preview.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v1.3.0...v2.0.0-preview.0) (2022-04-14)


### ⚠ BREAKING CHANGES

* New naming scheme rules:
All presets are prefixed with `Basic`.
Removed the `GraphType` part of classes as it made little sense.
Bases are now named in simple form. For example `PropertyValueBaseGraphType` --> `PropertyValue`

New naming of classes:

Maps:
BaseMap --> DictionaryMap

Content:
ContentGraphType --> BasicContent
IContentGraphTypeBase --> IContent

Content queries:
ContentQuery --> BasicContentQuery
ContentQueryBase --> ContentQuery

Content types:
ContentTypeGraphType --> ContentType

Elements:
ElementGraphType --> BasicElement
IElementGraphTypeBase --> IElement

Properties:
PropertyValueBaseGraphType --> PropertyValue
BlockListItemGraphType --> BasicBlockListItem
BlockListItemBaseGraphType --> BlockListItem
BlockListModelGraphType --> BasicBlockListModel
ContentPickerGraphType --> BasicContentPicker
ContentPickerItemGraphType --> BasicContentPickerItem
PropertyValueBasicGraphType --> BasicPropertyValue
MediaItem --> BasicMediaItem
MediaPickerGraphType --> BasicMediaPicker
MemberGraphType --> BasicMember
MemberPickerGraphType --> BasicMemberPicker
LinkGraphType --> BasicLink
MultiUrlPickerGraphType --> BasicMultiUrlPicker
ElementBaseGraphType --> NestedContentElement
NestedContentElementGraphType --> BasicNestedContentElement
NestedContentGraphType --> BasicNestedContent
RichTextEditorGraphType --> BasicRichText
IPropertyGraphTypeBase --> IProperty
PropertyGraphType --> BasicProperty

Property queries:
PropertyQuery --> BasicPropertyQuery
PropertyQueryBase --> PropertyQuery

Property types:
IPropertyTypeGraphType --> IPropertyType

Media:
IMediaGraphTypeBase --> IMedia
MediaGraphType --> BasicMedia

Media queries:
MediaQuery --> BasicMediaQuery
MediaQueryBase --> MediaQuery
* Changed parameters for most extensions to use option classes for the available options.
* Automapper was removed
* .Net 5 is no longer supported

### Features

* Created more developer friendly options to UHeadless extensions ([25b9dc6](https://github.com/nikcio/Nikcio.UHeadless/commit/25b9dc6a306d9b587435904dbbacde9a79534856))
* Removed Automapper ([349d148](https://github.com/nikcio/Nikcio.UHeadless/commit/349d148166292aad058d11030f73334dcfa29203))
* Steamline flow and models ([#35](https://github.com/nikcio/Nikcio.UHeadless/issues/35)) ([f2efc27](https://github.com/nikcio/Nikcio.UHeadless/commit/f2efc2798b212f1df99d9a9ad90ef74c1767286f))
* Updated names to be more clear and easier to remember ([2e6ff82](https://github.com/nikcio/Nikcio.UHeadless/commit/2e6ff8214485faf0320ddb3b4b829dc2c7fb718e))
* Updated to .Net 6 ([6aafada](https://github.com/nikcio/Nikcio.UHeadless/commit/6aafada3b88bc0a87bc08ffaa7b689007e1f19ff))


### Bug Fixes

* **deps:** Removed unnecessary dependencies ([f82feaf](https://github.com/nikcio/Nikcio.UHeadless/commit/f82feaf713b72206d9a8aa3aed536e4b6196299a))

## [1.3.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v1.2.0...v1.3.0) (2022-03-05)


### Features

* Added media queries ([dd4c079](https://github.com/nikcio/Nikcio.UHeadless/commit/dd4c0799fe2cebf38fda73fe91c76665c972e6a9))
* Added XML docs & nullable ([98eb8d8](https://github.com/nikcio/Nikcio.UHeadless/commit/98eb8d8556c4bd9ab0ed57232085b6363509cee8))

## [1.2.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v1.1.0...v1.2.0) (2022-03-01)


### Features

* Added virtual to all public methods ([03d400a](https://github.com/nikcio/Nikcio.UHeadless/commit/03d400a92f48469ef7f6902eb3d7078c0f914a1e))

## [1.1.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v1.0.0...v1.1.0) (2022-02-26)


### Features

* Added customizable queries & return types ([707843e](https://github.com/nikcio/Nikcio.UHeadless/commit/707843e90e0a0b10eb166c154d8137b297ad76fc))

## [1.0.0](https://github.com/nikcio/Nikcio.UHeadless/compare/v0.1.7...v1.0.0) (2022-02-06)


### ⚠ BREAKING CHANGES

* This changed almost all namespaces.
* Updated the naming of the queries to include what is being queried.

Example:
To get content at root you previously used atRoot this is now contentAtRoot

List:
atRoot --> contentAtRoot
byId --> contentById
byGuid --> contentByGuid
atRoute --> contentAtRoute

New:
propertiesAtRoute
propertiesById
propertiesByGuid

The new queries uses the same values for fetching properties but gives an eaiser way to do filtering, paging and sorting.

### Features

* Added HotChocolate.Data ([6eb7c67](https://github.com/nikcio/Nikcio.UHeadless/commit/6eb7c67e99de869db4ba42d4ffceb0457773d707))
* Added InitializeOnStartup to improve startup performance ([c4fa00f](https://github.com/nikcio/Nikcio.UHeadless/commit/c4fa00fa87b30d17b6d801a6a1c74b34edb830e7))
* Added option to throw on schema error ([519b89d](https://github.com/nikcio/Nikcio.UHeadless/commit/519b89d8b2944ac8931ef714cf76dc90b1f5b536))
* Added Paging, Filtering & Sorting ([8fe4483](https://github.com/nikcio/Nikcio.UHeadless/commit/8fe4483607c002a9baa732fd3b915f9afa682ff0))
* Added support for Media Picker ([e0ea5b8](https://github.com/nikcio/Nikcio.UHeadless/commit/e0ea5b8462c9763adf4b52d406dda24bf3d3c14f))
* Added Tracing option ([b083bc3](https://github.com/nikcio/Nikcio.UHeadless/commit/b083bc34efb5a2a8e5778c25b645d6afbd06b98e))
* Updated HotChocolate.AspNetCore to 12.6.0 ([667af04](https://github.com/nikcio/Nikcio.UHeadless/commit/667af041c0e830b95e25aaeede7f0167a66bf25e))


### Bug Fixes

* Added PropertyRepository to DI ([fa4382f](https://github.com/nikcio/Nikcio.UHeadless/commit/fa4382f8b8b0c41e35eda9710d82f2b4b752159e))
* Changes to improve code quality ([03db2cb](https://github.com/nikcio/Nikcio.UHeadless/commit/03db2cb0e93bb186520427b5a2def32b662a4d3c))
* Fixed filtering on ContentType, Key and Properties ([76edfe5](https://github.com/nikcio/Nikcio.UHeadless/commit/76edfe59f0b1345058a4ae60cb2b6661363fd77d))
* Fixed tracing option ([7bda7b1](https://github.com/nikcio/Nikcio.UHeadless/commit/7bda7b1909907385824222cb0aa73f668bc609f4))


* !refactor: Moved into feature code structure ([9645a32](https://github.com/nikcio/Nikcio.UHeadless/commit/9645a32e15449fe4de6435f02c4a124273f236a9))
* !feat: Added seperate property queries ([26e41b9](https://github.com/nikcio/Nikcio.UHeadless/commit/26e41b915875894d50a82eb53ec2f71bdb6240d3))

### [0.1.7](https://github.com/nikcio/Nikcio.UHeadless/compare/v0.1.6...v0.1.7) (2022-02-05)


### Features

* Added Cors options to the startup extension ([0c087d7](https://github.com/nikcio/Nikcio.UHeadless/commit/0c087d7fcbf9c5bba45fdec6563611f205c7dd73))


### Bug Fixes

* Made Cors option optional ([5cdebbc](https://github.com/nikcio/Nikcio.UHeadless/commit/5cdebbc6d63b84be95ec6541a40adb842ce310b5))

### [0.1.6](https://github.com/nikcio/Nikcio.UHeadless/compare/v0.1.5...v0.1.6) (2022-02-05)


### Bug Fixes

* Removed type property ([4307261](https://github.com/nikcio/Nikcio.UHeadless/commit/430726115b075efa39b588dca6775fccb7342384))

### [0.1.5](https://github.com/nikcio/Nikcio.UHeadless/compare/v0.1.4-beta...v0.1.5) (2022-02-04)


### Features

* Added Content Picker model ([432b71f](https://github.com/nikcio/Nikcio.UHeadless/commit/432b71f840a12b80ae062e7107e2c3685420c122))
* Added Member graph model ([c52a73b](https://github.com/nikcio/Nikcio.UHeadless/commit/c52a73bd97b49898f54b5b789f4cc061ebd4b303))
* Added MultiNodeTreePicker support ([82f0602](https://github.com/nikcio/Nikcio.UHeadless/commit/82f060296490ad46a623d2757a2fc23df412f7a5))
* Added MultiUrlPicker model ([f698814](https://github.com/nikcio/Nikcio.UHeadless/commit/f698814b6b8a1180cb3adb23715be4061614ba53))
* Made properties read-only on content ([a6443a3](https://github.com/nikcio/Nikcio.UHeadless/commit/a6443a34e238aba996ba821fdcc746312d38c978))


### Bug Fixes

* Fixed MultiUrlPicker initial value ([03bdc89](https://github.com/nikcio/Nikcio.UHeadless/commit/03bdc896d1cfd4378297ee13e945adf617699418))
* Fixed property map implementation ([564a6a8](https://github.com/nikcio/Nikcio.UHeadless/commit/564a6a87b4e6aab51fe3931a14fd569933940bf8))
* Fixed PropertyMap defaults and custom mappings ([f0b98f4](https://github.com/nikcio/Nikcio.UHeadless/commit/f0b98f4d64bde9cd366a6a6604af9a717d060dee))

### [0.1.4](https://github.com/nikcio/Nikcio.UHeadless/compare/v0.1.3-beta...v0.1.4) (2022-01-27)


### Features

* Added support for Rich text editor ([67db76d](https://github.com/nikcio/Nikcio.UHeadless/commit/67db76db6f1ccc499f709fe1ce515ff7a6d6cbe2))


### Bug Fixes

* Fixed automapper error when fetching content ([bb51076](https://github.com/nikcio/Nikcio.UHeadless/commit/bb51076ebb97e8687a0564b79d389062b88f35f8))
* Fixed children fetching ([8f79eb7](https://github.com/nikcio/Nikcio.UHeadless/commit/8f79eb7651d6422b6fcf5041fdd0dabb487d4212))

### [0.1.3](https://github.com/nikcio/Nikcio.UHeadless/compare/v0.1.2...v0.1.3) (2022-01-27)


### Features

* Added automapper extension method ([bef3476](https://github.com/nikcio/Nikcio.UHeadless/commit/bef34764f22bce9579d6a96439cad39e83b8240d))
* Added depencency reflector factory ([db8236b](https://github.com/nikcio/Nikcio.UHeadless/commit/db8236b498692367b5a0984cca9b969e9952bbe6))
* Block list & Nested content can now use any type ([a4585d0](https://github.com/nikcio/Nikcio.UHeadless/commit/a4585d0a382803d2e39153f2e361001458dee7d3))


### Bug Fixes

* Added required constructor to PropertyValueBaseGraphType ([9d7e12a](https://github.com/nikcio/Nikcio.UHeadless/commit/9d7e12a3b10e9a141b30a98a2e08658ce3eb7f62))
* Added support for Umbraco v9 ([977b237](https://github.com/nikcio/Nikcio.UHeadless/commit/977b237904dab5460208ca4a592cf3ca2065e53a))
* Made culture and content available on CreateProperty ([eb10b59](https://github.com/nikcio/Nikcio.UHeadless/commit/eb10b59a362f45007393ec994f757b27f8a047a6))

### 0.1.2 (2022-01-25)


### Features

* Added DI to property value generation ([c8f6705](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/c8f67056b5c29a6d7ea24d48bf0ab632f3de5c52))
* Added nested content support & Added properties to block list ([ef655e9](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/ef655e9480ba7ad3ed7c9891df1c3140a8378b90))
* Added property value mapping options ([1ff1112](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/1ff1112a2e326bd247eaeb96450aecef31514efe))
* Added standard-version ([48be288](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/48be2889f41d0ada781292486a05daef7aaf4ef7))
* Added the abillity to fetch properties ([d5d83d8](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/d5d83d8e8411973ddf7209826007cab8f433da7d))
* Content fetching 1.0 ([eb8177f](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/eb8177f7291cb22b09d583407343b10d72f2032a))
* Created extensions for startup ([76290a0](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/76290a0dca7b2f6295accc0d36272369fbef302b))
* Renamed project ([bd62b60](https://github.com/nikcio/Nikcio.Umbraco.Headless/commit/bd62b6062e7e4f5fad0d0222489808c74f71c3a9))
