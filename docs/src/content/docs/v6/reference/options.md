---
title: UHeadless Options
description: Overview of the options in Nikcio.UHeadless.
---

The `UHeadlessOptions` class provides configuration options for the UHeadless package. These options can be used to customize various aspects of UHeadless behavior.

## UHeadlessOptions Properties

| Property                  | Description                                                                             |
|---------------------------|-----------------------------------------------------------------------------------------|
| PropertyMap               | Used to register custom models to specific property values.                             |
| DisableAuthorization      | Disables authorization for the GraphQL server.                                          |
| RequestExecutorBuilder    | Used to customize the GraphQL server.                                                   |

## Extensions

| Property               | Description                                                                                |
|------------------------|--------------------------------------------------------------------------------------------|
| AddDefaults            | Adds default property mappings and services to be used for the default queries.            |
| AddAuth                | Adds the authentication services to the UHeadless package.                                 |
| AddQuery               | Adds a query to the GraphQL schema                                                         |
| AddMutation            | Adds a mutation to the GraphQL schema                                                      |
| AddEditorMapping       | Adds a mapping of a type to a editor alias.                                                |
| AddAliasMapping        | Adds a mapping of a type to a content type alias combined with a property type alias.      |
| RemoveAliasMapping     | Removes a alias mapping                                                                    |
| RemoveEditorMapping    | Removes a editor mapping                                                                   |