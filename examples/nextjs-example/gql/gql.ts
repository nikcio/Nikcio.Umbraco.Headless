/* eslint-disable */
import * as types from './graphql';
import { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';

/**
 * Map of all GraphQL operations in the project.
 *
 * This map has several performance disadvantages:
 * 1. It is not tree-shakeable, so it will include all operations in the project.
 * 2. It is not minifiable, so the string of a GraphQL query will be multiple times inside the bundle.
 * 3. It does not support dead code elimination, so it will add unused operations.
 *
 * Therefore it is highly recommended to use the babel or swc plugin for production.
 */
const documents = {
    "\n    query contentTypeQuery($baseUrl: String!, $route: String!, $culture: String, $includePreview: Boolean!) {\n        contentByRoute(\n            baseUrl: $baseUrl\n            route: $route\n            inContext: { culture: $culture, includePreview: $includePreview }\n        ) {\n            name\n            url(urlMode: ABSOLUTE)\n            properties {\n                __typename\n            }\n        }\n    }\n": types.ContentTypeQueryDocument,
    "\n    mutation createToken($scope: Any) {\n        createToken(claims: [{ name: \"headless-scope\", value: $scope }]) {\n            expires\n            header\n            prefix\n            token\n        }\n    }\n": types.CreateTokenDocument,
};

/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 *
 *
 * @example
 * ```ts
 * const query = graphql(`query GetUser($id: ID!) { user(id: $id) { name } }`);
 * ```
 *
 * The query argument is unknown!
 * Please regenerate the types.
 */
export function graphql(source: string): unknown;

/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n    query contentTypeQuery($baseUrl: String!, $route: String!, $culture: String, $includePreview: Boolean!) {\n        contentByRoute(\n            baseUrl: $baseUrl\n            route: $route\n            inContext: { culture: $culture, includePreview: $includePreview }\n        ) {\n            name\n            url(urlMode: ABSOLUTE)\n            properties {\n                __typename\n            }\n        }\n    }\n"): (typeof documents)["\n    query contentTypeQuery($baseUrl: String!, $route: String!, $culture: String, $includePreview: Boolean!) {\n        contentByRoute(\n            baseUrl: $baseUrl\n            route: $route\n            inContext: { culture: $culture, includePreview: $includePreview }\n        ) {\n            name\n            url(urlMode: ABSOLUTE)\n            properties {\n                __typename\n            }\n        }\n    }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n    mutation createToken($scope: Any) {\n        createToken(claims: [{ name: \"headless-scope\", value: $scope }]) {\n            expires\n            header\n            prefix\n            token\n        }\n    }\n"): (typeof documents)["\n    mutation createToken($scope: Any) {\n        createToken(claims: [{ name: \"headless-scope\", value: $scope }]) {\n            expires\n            header\n            prefix\n            token\n        }\n    }\n"];

export function graphql(source: string) {
  return (documents as any)[source] ?? {};
}

export type DocumentType<TDocumentNode extends DocumentNode<any, any>> = TDocumentNode extends DocumentNode<  infer TType,  any>  ? TType  : never;