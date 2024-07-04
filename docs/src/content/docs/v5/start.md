---
title: Nikcio.UHeadless Documentation - Version 5.0.0+
description: Welcome to version 5 of the Nikcio.UHeadless documentation! This documentation aims to provide you with comprehensive resources to help you get started and make the most out of the Nikcio.UHeadless package.
---

Welcome to the documentation for Nikcio.UHeadless version 5.0.0! This documentation aims to provide you with comprehensive resources to help you get started and make the most out of the Nikcio.UHeadless package. Whether you're new to the package or looking to extend its functionality, we've got you covered.

## Fundamentals

In this section, you will find essential information about Nikcio.UHeadless and its core concepts. It covers topics such as extending Nikcio.UHeadless and important security considerations.

### Getting Started
- [Getting started](../fundamentals/getting-started): Find step-by-step instructions on how to install Nikcio.UHeadless and get started with querying content, media, and members.
  - [Querying content](../fundamentals/querying/content): Explore the query options for content.
  - [Querying media](../fundamentals/querying/media): Explore the query options for media.
  - [Querying members](../fundamentals/querying/members): Explore the query options for members.
  - [Querying properties](../fundamentals/querying/content): Discover how to query properties on content, media and members.
- [Security Considerations](../fundamentals/security): Explore important security considerations when using Nikcio.UHeadless.

## Extending Nikcio.UHeadless

This section focuses on extending Nikcio.UHeadless to tailor it to your project's needs. It covers different areas where you can extend Nikcio.UHeadless, including content, media, members, integrations, and properties.

### Content

- [Extending Content](../extending/content): Learn how to extend the existing content model and how build your own in Nikcio.UHeadless.

### Media

- [Extending Media](../extending/media): Discover how to extend the existing media model and how build your own in Nikcio.UHeadless.

### Members

- [Extending Members](../extending/member): Learn how to extend the existing member model and how build your own in Nikcio.UHeadless.

### Integrations

- [Skybrud Redirects](../extending/skybrud-redirects): Explore how to integrate Nikcio.UHeadless with [Skybrud Redirects](https://marketplace.umbraco.com/package/skybrud.umbraco.redirects) for enhanced functionality.
- [Url Tracker](../extending/url-tracker): Learn how to integrate Nikcio.UHeadless with [Url Tracker](https://marketplace.umbraco.com/package/urltracker) for enhanced functionality.

### Performance

- Persisted queries: Learn how to use persisted queries to improve performance when querying content, media, and members in Nikcio.UHeadless.
  - HotChocolate has a great overview of how to integrate persisted queries in their [documentation](https://chillicream.com/docs/hotchocolate/v13/performance/#persisted-queries). Use add any extension methods to the `IRequestExecutorBuilder` in the `Program.cs` file where you configure UHeadless.

### Properties

- [Overview of Properties](../extending/properties/overview): Get an overview of the different property types and their usage in Nikcio.UHeadless.
- [Block List](../extending/properties/block-list): Discover how to extend the block list property model in Nikcio.UHeadless.
- [Custom Editor](../extending/properties/custom-editor): Learn how to use custom property editors in Nikcio.UHeadless.
- [Media Picker](../extending/properties/media-picker): Explore how to extend the media picker property model in Nikcio.UHeadless.
- [Rich Text](../extending/properties/rich-text): Learn how to extend the rich text property model in Nikcio.UHeadless.

## Reference

In the reference section, you will find detailed information about various aspects of Nikcio.UHeadless, including options, content, media, members, and properties.

- [Options](../reference/options): Learn about the options available for configuring Nikcio.UHeadless.
- [Endpoint Options](../reference/endpoint-options): Explore the available options for configuring the Nikcio.UHeadless endpoint.

- [Content Reference](../reference/content): Find reference documentation for working with content in Nikcio.UHeadless.
- [Media Reference](../reference/media): Find reference documentation for working with media in Nikcio.UHeadless.
- [Members Reference](../reference/members): Find reference documentation for working with members in Nikcio.UHeadless.
- [Properties Reference](../reference/properties): Find reference documentation for working with properties in Nikcio.UHeadless.

We hope this documentation helps you make the most out of Nikcio.UHeadless. If you have any questions or need further assistance, don't hesitate to reach out to us.

**Enjoy building a headless GraphQL interface with Nikcio.UHeadless!**

---

For those interested in supporting the development of Nikcio.UHeadless, consider becoming a sponsor on [GitHub Sponsors](https://github.com/sponsors/nikcio/). Your sponsorship helps us continue to improve and maintain this package. Thank you for your support!