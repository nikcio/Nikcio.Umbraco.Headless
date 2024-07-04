/* eslint-disable */
import { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  Any: { input: any; output: any; }
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: { input: any; output: any; }
  /** The `Long` scalar type represents non-fractional signed whole 64-bit numeric values. Long can represent values between -(2^63) and 2^63 - 1. */
  Long: { input: any; output: any; }
  UUID: { input: any; output: any; }
};

export enum ApplyPolicy {
  AfterResolver = 'AFTER_RESOLVER',
  BeforeResolver = 'BEFORE_RESOLVER',
  Validation = 'VALIDATION'
}

export type Article = IArticle & IArticleControls & IContentControls & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'Article';
  /** Enter the date for the article */
  articleDate?: Maybe<DateTimePicker>;
  author?: Maybe<ContentPicker>;
  categories?: Maybe<ContentPicker>;
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ArticleControls = IArticleControls & {
  __typename?: 'ArticleControls';
  /** Enter the date for the article */
  articleDate?: Maybe<DateTimePicker>;
  author?: Maybe<ContentPicker>;
  categories?: Maybe<ContentPicker>;
};

export type ArticleList = IArticleList & IContentControls & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'ArticleList';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type Author = IAuthor & IContentControls & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'Author';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type AuthorList = IAuthorList & IContentControls & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'AuthorList';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

/** Represents a block grid property value. */
export type BlockGrid = PropertyValue & {
  __typename?: 'BlockGrid';
  /** Gets the blocks of a block grid model. */
  blocks?: Maybe<Array<BlockGridItem>>;
  /** Gets the number of columns defined for the grid. */
  gridColumns?: Maybe<Scalars['Int']['output']>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** Represents a block grid area. */
export type BlockGridArea = {
  __typename?: 'BlockGridArea';
  /** Gets the alias of the block grid area. */
  alias: Scalars['String']['output'];
  /** Gets the blocks of the block grid area. */
  blocks?: Maybe<Array<BlockGridItem>>;
  /** Gets the column dimensions of the block. */
  columnSpan: Scalars['Int']['output'];
  /** Gets the row dimensions of the block. */
  rowSpan: Scalars['Int']['output'];
};

/** Represents a block grid item. */
export type BlockGridItem = {
  __typename?: 'BlockGridItem';
  /** Gets the areas of the block grid item. */
  areas: Array<BlockGridArea>;
  /** Gets the column dimensions of the block. */
  columnSpan: Scalars['Int']['output'];
  /** Gets the alias of the content block grid item. */
  contentAlias?: Maybe<Scalars['String']['output']>;
  /** Gets the content properties of the block grid item. */
  contentProperties: TypedBlockGridContentProperties;
  /** Gets the row dimensions of the block. */
  rowSpan: Scalars['Int']['output'];
  /** Gets the alias of the settings block grid item. */
  settingsAlias?: Maybe<Scalars['String']['output']>;
  /** Gets the setting properties of the block grid item. */
  settingsProperties: TypedBlockGridSettingsProperties;
};

/** Represents a block list model. */
export type BlockList = PropertyValue & {
  __typename?: 'BlockList';
  /** Gets the blocks of a block list model. */
  blocks?: Maybe<Array<BlockListItem>>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** Represents a block list item. */
export type BlockListItem = {
  __typename?: 'BlockListItem';
  /** Gets the alias of the content block list item. */
  contentAlias?: Maybe<Scalars['String']['output']>;
  /** Gets the content properties of the block list item. */
  contentProperties: TypedBlockListContentProperties;
  /** Gets the alias of the settings block list item. */
  settingsAlias?: Maybe<Scalars['String']['output']>;
  /** Gets the setting properties of the block list item. */
  settingsProperties: TypedBlockListSettingsProperties;
};

export type CategoryList = ICategoryList & IVisibilityControls & {
  __typename?: 'CategoryList';
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type CodeSnippetRow = ICodeSnippetRow & {
  __typename?: 'CodeSnippetRow';
  code?: Maybe<DefaultProperty>;
  /** Enter a name for this code snippet */
  title?: Maybe<DefaultProperty>;
};

export type CodeSnippetRowSettings = ICodeSnippetRowSettings & IHideProperty & ISpacingProperties & {
  __typename?: 'CodeSnippetRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type Contact = IContact & IContactFormControls & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'Contact';
  /** Enter the message to show on error */
  errorMessage?: Maybe<RichText>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Enter the message to tell the user what to do */
  instructionMessage?: Maybe<RichText>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the message to show on success */
  successMessage?: Maybe<RichText>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ContactFormControls = IContactFormControls & {
  __typename?: 'ContactFormControls';
  /** Enter the message to show on error */
  errorMessage?: Maybe<RichText>;
  /** Enter the message to tell the user what to do */
  instructionMessage?: Maybe<RichText>;
  /** Enter the message to show on success */
  successMessage?: Maybe<RichText>;
};

export type Content = IContent & IContentControls & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'Content';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ContentControls = IContentControls & {
  __typename?: 'ContentControls';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
};

export type ContentItem = {
  __typename?: 'ContentItem';
  /** Gets the id of a content item. */
  id?: Maybe<Scalars['Int']['output']>;
  /** Gets the key of a content item. */
  key?: Maybe<Scalars['UUID']['output']>;
  /** Gets the name of a content item. */
  name?: Maybe<Scalars['String']['output']>;
  /** Gets the parent of the content item. */
  parent?: Maybe<ContentItem>;
  /** Gets the properties of the content item. */
  properties: TypedProperties;
  /** Gets the redirect information for the content item. */
  redirect?: Maybe<RedirectInfo>;
  statusCode: Scalars['Int']['output'];
  /** Gets the identifier of the template to use to render the content item. */
  templateId?: Maybe<Scalars['Int']['output']>;
  /** Gets the date the content item was last updated. */
  updateDate?: Maybe<Scalars['DateTime']['output']>;
  /** Gets the url of a content item. */
  url?: Maybe<Scalars['String']['output']>;
  /** Gets the url segment of the content item. */
  urlSegment?: Maybe<Scalars['String']['output']>;
};


export type ContentItemUrlArgs = {
  urlMode: UrlMode;
};

/** Represents a content picker value. */
export type ContentPicker = PropertyValue & {
  __typename?: 'ContentPicker';
  /** Gets the content items of a picker. */
  items?: Maybe<Array<ContentPickerItem>>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** Represents a content picker item. */
export type ContentPickerItem = {
  __typename?: 'ContentPickerItem';
  /** Gets the id of a content item. */
  id: Scalars['Int']['output'];
  /** Gets the key of a content item. */
  key: Scalars['UUID']['output'];
  /** Gets the name of a content item. */
  name?: Maybe<Scalars['String']['output']>;
  /** Gets the properties of the content item. */
  properties: TypedProperties;
  /** Gets the url of a content item. */
  url: Scalars['String']['output'];
  /** Gets the url segment of the content item. */
  urlSegment?: Maybe<Scalars['String']['output']>;
};


/** Represents a content picker item. */
export type ContentPickerItemUrlArgs = {
  urlMode: UrlMode;
};

/** Represents a date time property value. */
export type DateTimePicker = PropertyValue & {
  __typename?: 'DateTimePicker';
  /** The model of the property value */
  model: Scalars['String']['output'];
  /** Gets the value of the property. */
  value?: Maybe<Scalars['DateTime']['output']>;
};

/** A catch all property value that simply returns the value of the property. This is all that is needed for simple properties that doesn't need any special handling or formatting. */
export type DefaultProperty = PropertyValue & {
  __typename?: 'DefaultProperty';
  /** The model of the property value */
  model: Scalars['String']['output'];
  /** Gets the value of the property. */
  value?: Maybe<Scalars['Any']['output']>;
};

/** Represents a content type that doesn't have any properties and therefore needs a placeholder */
export type EmptyPropertyType = {
  __typename?: 'EmptyPropertyType';
  /** Placeholder field. Will never hold a value. */
  Empty_Field: Scalars['String']['output'];
};

export type Error = IContentControls & IError & IHeaderControls & IMainImageControls & IseoControls & IVisibilityControls & {
  __typename?: 'Error';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type File = IFile & {
  __typename?: 'File';
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type FooterControls = IFooterControls & {
  __typename?: 'FooterControls';
  /** Add any social links using the SVG icons */
  socialIconLinks?: Maybe<BlockList>;
};

export type HeaderControls = IHeaderControls & {
  __typename?: 'HeaderControls';
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
};

export type HideProperty = IHideProperty & {
  __typename?: 'HideProperty';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
};

export type Home = IContentControls & IFooterControls & IHeaderControls & IHome & IMainImageControls & IseoControls & {
  __typename?: 'Home';
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Add any social links using the SVG icons */
  socialIconLinks?: Maybe<BlockList>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
};

export type IArticle = {
  /** Enter the date for the article */
  articleDate?: Maybe<DateTimePicker>;
  author?: Maybe<ContentPicker>;
  categories?: Maybe<ContentPicker>;
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IArticleControls = {
  /** Enter the date for the article */
  articleDate?: Maybe<DateTimePicker>;
  author?: Maybe<ContentPicker>;
  categories?: Maybe<ContentPicker>;
};

export type IArticleList = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IAuthor = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IAuthorList = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ICategoryList = {
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ICodeSnippetRow = {
  code?: Maybe<DefaultProperty>;
  /** Enter a name for this code snippet */
  title?: Maybe<DefaultProperty>;
};

export type ICodeSnippetRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type IContact = {
  /** Enter the message to show on error */
  errorMessage?: Maybe<RichText>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Enter the message to tell the user what to do */
  instructionMessage?: Maybe<RichText>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the message to show on success */
  successMessage?: Maybe<RichText>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IContactFormControls = {
  /** Enter the message to show on error */
  errorMessage?: Maybe<RichText>;
  /** Enter the message to tell the user what to do */
  instructionMessage?: Maybe<RichText>;
  /** Enter the message to show on success */
  successMessage?: Maybe<RichText>;
};

export type IContent = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IContentControls = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
};

export type IError = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IFile = {
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type IFooterControls = {
  /** Add any social links using the SVG icons */
  socialIconLinks?: Maybe<BlockList>;
};

export type IHeaderControls = {
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
};

export type IHideProperty = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
};

export type IHome = {
  /** Add the rows of content for the page */
  contentRows?: Maybe<BlockList>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Add any social links using the SVG icons */
  socialIconLinks?: Maybe<BlockList>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
};

export type IIconLinkRow = {
  /** Choose the icon for this item. It must be an SVG */
  icon?: Maybe<MediaPicker>;
  /** Enter your link for this item */
  link?: Maybe<MultiUrlPicker>;
};

export type IIconLinkRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
};

export type IImage = {
  /** Describe the image's purpose and content in a concise and informative way. */
  altText?: Maybe<DefaultProperty>;
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
  /** in pixels */
  umbracoHeight?: Maybe<Label>;
  /** in pixels */
  umbracoWidth?: Maybe<Label>;
};

export type IImageCarouselRow = {
  /** Choose the images for the carousel row */
  images?: Maybe<MediaPicker>;
};

export type IImageCarouselRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type IImageRow = {
  /** Enter a caption for the image */
  caption?: Maybe<DefaultProperty>;
  /** Add the image for this row */
  image?: Maybe<MediaPicker>;
};

export type IImageRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type ILatestArticlesRow = {
  /** Choose the parent page where you want to display articles from */
  articleList?: Maybe<ContentPicker>;
  /** Choose the amount of articles to display per page */
  pageSize?: Maybe<DefaultProperty>;
  /** Set this to true if you would like to show the pagination for these articles */
  showPagination?: Maybe<DefaultProperty>;
};

export type ILatestArticlesRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type IMainImageControls = {
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
};

export type IMember = {
  umbracoMemberApproved?: Maybe<DefaultProperty>;
  umbracoMemberComments?: Maybe<DefaultProperty>;
  umbracoMemberFailedPasswordAttempts?: Maybe<Label>;
  umbracoMemberLastLockoutDate?: Maybe<Label>;
  umbracoMemberLastLogin?: Maybe<Label>;
  umbracoMemberLastPasswordChangeDate?: Maybe<Label>;
  umbracoMemberLockedOut?: Maybe<DefaultProperty>;
};

export type IRichTextRow = {
  /** Enter the content for this rich text item */
  content?: Maybe<RichText>;
};

export type IRichTextRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type IseoControls = {
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
};

export type ISearch = {
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ISpacingProperties = {
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type IUmbracoMediaArticle = {
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type IUmbracoMediaAudio = {
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type IUmbracoMediaVectorGraphics = {
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type IUmbracoMediaVideo = {
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type IVideoRow = {
  /** Add a caption to display under the video if you would like */
  caption?: Maybe<DefaultProperty>;
  /** Add the YouTube Url in here */
  videoUrl?: Maybe<DefaultProperty>;
};

export type IVideoRowSettings = {
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type IVisibilityControls = {
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IxmlSitemap = {
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type IconLinkRow = IIconLinkRow & {
  __typename?: 'IconLinkRow';
  /** Choose the icon for this item. It must be an SVG */
  icon?: Maybe<MediaPicker>;
  /** Enter your link for this item */
  link?: Maybe<MultiUrlPicker>;
};

export type IconLinkRowSettings = IHideProperty & IIconLinkRowSettings & {
  __typename?: 'IconLinkRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
};

export type Image = IImage & {
  __typename?: 'Image';
  /** Describe the image's purpose and content in a concise and informative way. */
  altText?: Maybe<DefaultProperty>;
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
  /** in pixels */
  umbracoHeight?: Maybe<Label>;
  /** in pixels */
  umbracoWidth?: Maybe<Label>;
};

export type ImageCarouselRow = IImageCarouselRow & {
  __typename?: 'ImageCarouselRow';
  /** Choose the images for the carousel row */
  images?: Maybe<MediaPicker>;
};

export type ImageCarouselRowSettings = IHideProperty & IImageCarouselRowSettings & ISpacingProperties & {
  __typename?: 'ImageCarouselRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type ImageRow = IImageRow & {
  __typename?: 'ImageRow';
  /** Enter a caption for the image */
  caption?: Maybe<DefaultProperty>;
  /** Add the image for this row */
  image?: Maybe<MediaPicker>;
};

export type ImageRowSettings = IHideProperty & IImageRowSettings & ISpacingProperties & {
  __typename?: 'ImageRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

/** A JWT token. */
export type JwtToken = {
  __typename?: 'JwtToken';
  /** The expiration time of the token in Unix timestamp. */
  expires: Scalars['Long']['output'];
  /** The header used when using the token. */
  header: Scalars['String']['output'];
  /** The prefix used when using the token. */
  prefix: Scalars['String']['output'];
  /** The JWT token. */
  token: Scalars['String']['output'];
};

export type Label = PropertyValue & {
  __typename?: 'Label';
  /** The model of the property value */
  model: Scalars['String']['output'];
  /** Gets the value of the property. */
  value?: Maybe<Scalars['Any']['output']>;
};

export type LatestArticlesRow = ILatestArticlesRow & {
  __typename?: 'LatestArticlesRow';
  /** Choose the parent page where you want to display articles from */
  articleList?: Maybe<ContentPicker>;
  /** Choose the amount of articles to display per page */
  pageSize?: Maybe<DefaultProperty>;
  /** Set this to true if you would like to show the pagination for these articles */
  showPagination?: Maybe<DefaultProperty>;
};

export type LatestArticlesRowSettings = IHideProperty & ILatestArticlesRowSettings & ISpacingProperties & {
  __typename?: 'LatestArticlesRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export enum LinkType {
  Content = 'CONTENT',
  External = 'EXTERNAL',
  Media = 'MEDIA'
}

export type MainImageControls = IMainImageControls & {
  __typename?: 'MainImageControls';
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
};

/** Represents a media picker item. */
export type MediaPicker = PropertyValue & {
  __typename?: 'MediaPicker';
  /** Gets the media items of a picker. */
  mediaItems: Array<MediaPickerItem>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** Represents a media item. */
export type MediaPickerItem = {
  __typename?: 'MediaPickerItem';
  /** Gets the id of a media item. */
  id: Scalars['Int']['output'];
  /** Gets the key of a media item. */
  key: Scalars['UUID']['output'];
  /** Gets the name of a media item. */
  name?: Maybe<Scalars['String']['output']>;
  /** Gets the properties of the media item. */
  properties: TypedProperties;
  /** Gets the url of a media item. */
  url: Scalars['String']['output'];
  /** Gets the url segment of the media item. */
  urlSegment?: Maybe<Scalars['String']['output']>;
};


/** Represents a media item. */
export type MediaPickerItemUrlArgs = {
  urlMode: UrlMode;
};

export type Member = IMember & {
  __typename?: 'Member';
  umbracoMemberApproved?: Maybe<DefaultProperty>;
  umbracoMemberComments?: Maybe<DefaultProperty>;
  umbracoMemberFailedPasswordAttempts?: Maybe<Label>;
  umbracoMemberLastLockoutDate?: Maybe<Label>;
  umbracoMemberLastLogin?: Maybe<Label>;
  umbracoMemberLastPasswordChangeDate?: Maybe<Label>;
  umbracoMemberLockedOut?: Maybe<DefaultProperty>;
};

/** Represents a member picker. */
export type MemberPicker = PropertyValue & {
  __typename?: 'MemberPicker';
  /** Gets the member items of a picker. Requires the property.values.member.picker or global.member.read claim to access */
  members: Array<MemberPickerItem>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** Represents a member item. */
export type MemberPickerItem = {
  __typename?: 'MemberPickerItem';
  /** Gets the id of a member item. */
  id: Scalars['Int']['output'];
  /** Gets the key of a member item. */
  key: Scalars['UUID']['output'];
  /** Gets the name of a member item. */
  name?: Maybe<Scalars['String']['output']>;
  /** Gets the properties of the member item. */
  properties: TypedProperties;
};

/** Represents a multi url picker. */
export type MultiUrlPicker = PropertyValue & {
  __typename?: 'MultiUrlPicker';
  /** Gets the links of the picker. */
  links: Array<MultiUrlPickerItem>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** Represents a content item. */
export type MultiUrlPickerItem = {
  __typename?: 'MultiUrlPickerItem';
  /** Gets the id of a content item. */
  id?: Maybe<Scalars['Int']['output']>;
  /** Gets the key of a content item. */
  key?: Maybe<Scalars['UUID']['output']>;
  /** Gets the name of a content item. */
  name?: Maybe<Scalars['String']['output']>;
  /** Gets the properties of the content item. */
  properties: TypedProperties;
  /** Gets the target of the link. */
  target?: Maybe<Scalars['String']['output']>;
  /** Gets the type of the link. */
  type: LinkType;
  /** Gets the url of a content item. If the link isn't to a content item or media item then the UrlMode doesn't affect the url. */
  url: Scalars['String']['output'];
  /** Gets the url segment of the content item. */
  urlSegment?: Maybe<Scalars['String']['output']>;
};


/** Represents a content item. */
export type MultiUrlPickerItemUrlArgs = {
  urlMode: UrlMode;
};

/** The base mutation object */
export type Mutation = {
  __typename?: 'Mutation';
  /** Creates a JWT token to be used for other queries. */
  createToken: JwtToken;
};


/** The base mutation object */
export type MutationCreateTokenArgs = {
  claims: Array<TokenClaimInput>;
};

/** Represents nested content. */
export type NestedContent = PropertyValue & {
  __typename?: 'NestedContent';
  /** Gets the elements of a nested content. */
  elements: Array<NestedContentItem>;
  /** The model of the property value */
  model: Scalars['String']['output'];
};

export type NestedContentItem = {
  __typename?: 'NestedContentItem';
  /** Gets the properties of the nested content. */
  properties: TypedNestedContentProperties;
};

/** Represents a property fallback strategy */
export enum PropertyFallback {
  /** Fallback to tree ancestors */
  Ancestors = 'ANCESTORS',
  /** Fallback to default value */
  DefaultValue = 'DEFAULT_VALUE',
  /** Fallback to other languages */
  Language = 'LANGUAGE',
  /** Do not fallback */
  None = 'NONE'
}

/** A base for property values */
export type PropertyValue = {
  /** The model of the property value */
  model: Scalars['String']['output'];
};

/** The base query object */
export type Query = {
  __typename?: 'Query';
  /** Gets a content item by a route. */
  contentByRoute?: Maybe<ContentItem>;
};


/** The base query object */
export type QueryContentByRouteArgs = {
  baseUrl?: Scalars['String']['input'];
  inContext?: InputMaybe<QueryContextInput>;
  route: Scalars['String']['input'];
};

/** Represents the context of a query. */
export type QueryContextInput = {
  /** The culture of the query. */
  culture?: InputMaybe<Scalars['String']['input']>;
  /** The fallbacks to use on a property value. */
  fallbacks?: InputMaybe<Array<PropertyFallback>>;
  /** Whether to include preview content. */
  includePreview?: InputMaybe<Scalars['Boolean']['input']>;
  /** The segment to use on a property value. */
  segment?: InputMaybe<Scalars['String']['input']>;
};

export type RedirectInfo = {
  __typename?: 'RedirectInfo';
  isPermanent: Scalars['Boolean']['output'];
  redirectUrl?: Maybe<Scalars['String']['output']>;
};

/** Represents a rich text editor. */
export type RichText = PropertyValue & {
  __typename?: 'RichText';
  /** The model of the property value */
  model: Scalars['String']['output'];
  /** Gets the original value of the rich text editor or markdown editor. */
  sourceValue?: Maybe<Scalars['String']['output']>;
  /** Gets the HTML value of the rich text editor or markdown editor. */
  value?: Maybe<Scalars['String']['output']>;
};

export type RichTextRow = IRichTextRow & {
  __typename?: 'RichTextRow';
  /** Enter the content for this rich text item */
  content?: Maybe<RichText>;
};

export type RichTextRowSettings = IHideProperty & IRichTextRowSettings & ISpacingProperties & {
  __typename?: 'RichTextRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type SeoControls = IseoControls & {
  __typename?: 'SEOControls';
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
};

export type Search = IHeaderControls & IMainImageControls & IseoControls & ISearch & IVisibilityControls & {
  __typename?: 'Search';
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Choose the main image for this page */
  mainImage?: Maybe<MediaPicker>;
  /** Enter the meta description for this page */
  metaDescription?: Maybe<DefaultProperty>;
  /** Enter the keywords for this page */
  metaKeywords?: Maybe<DefaultProperty>;
  /** Enter the meta name for this page */
  metaName?: Maybe<DefaultProperty>;
  /** Enter a subtitle for this page */
  subtitle?: Maybe<DefaultProperty>;
  /** Enter the title for the page. If this is empty the name of the page will be used. */
  title?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type SpacingProperties = ISpacingProperties & {
  __typename?: 'SpacingProperties';
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

/** A claim for a token. */
export type TokenClaimInput = {
  /** The name of the claim. */
  name: Scalars['String']['input'];
  /** The type of claim. */
  type?: InputMaybe<TokenClaimType>;
  /** The value of the claim. */
  value?: InputMaybe<Scalars['Any']['input']>;
};

/** The type of claim. */
export enum TokenClaimType {
  Json = 'JSON',
  JsonArray = 'JSON_ARRAY'
}

/** Used to get typed properties on a block grid property for the content property */
export type TypedBlockGridContentProperties = Article | ArticleControls | ArticleList | Author | AuthorList | CategoryList | CodeSnippetRow | CodeSnippetRowSettings | Contact | ContactFormControls | Content | ContentControls | EmptyPropertyType | Error | File | FooterControls | HeaderControls | HideProperty | Home | IconLinkRow | IconLinkRowSettings | Image | ImageCarouselRow | ImageCarouselRowSettings | ImageRow | ImageRowSettings | LatestArticlesRow | LatestArticlesRowSettings | MainImageControls | Member | RichTextRow | RichTextRowSettings | SeoControls | Search | SpacingProperties | UmbracoMediaArticle | UmbracoMediaAudio | UmbracoMediaVectorGraphics | UmbracoMediaVideo | VideoRow | VideoRowSettings | VisibilityControls | XmlSitemap;

/** Used to get typed properties on a block grid property for the settings property */
export type TypedBlockGridSettingsProperties = Article | ArticleControls | ArticleList | Author | AuthorList | CategoryList | CodeSnippetRow | CodeSnippetRowSettings | Contact | ContactFormControls | Content | ContentControls | EmptyPropertyType | Error | File | FooterControls | HeaderControls | HideProperty | Home | IconLinkRow | IconLinkRowSettings | Image | ImageCarouselRow | ImageCarouselRowSettings | ImageRow | ImageRowSettings | LatestArticlesRow | LatestArticlesRowSettings | MainImageControls | Member | RichTextRow | RichTextRowSettings | SeoControls | Search | SpacingProperties | UmbracoMediaArticle | UmbracoMediaAudio | UmbracoMediaVectorGraphics | UmbracoMediaVideo | VideoRow | VideoRowSettings | VisibilityControls | XmlSitemap;

/** Used to get typed properties on a block list property for the content property */
export type TypedBlockListContentProperties = Article | ArticleControls | ArticleList | Author | AuthorList | CategoryList | CodeSnippetRow | CodeSnippetRowSettings | Contact | ContactFormControls | Content | ContentControls | EmptyPropertyType | Error | File | FooterControls | HeaderControls | HideProperty | Home | IconLinkRow | IconLinkRowSettings | Image | ImageCarouselRow | ImageCarouselRowSettings | ImageRow | ImageRowSettings | LatestArticlesRow | LatestArticlesRowSettings | MainImageControls | Member | RichTextRow | RichTextRowSettings | SeoControls | Search | SpacingProperties | UmbracoMediaArticle | UmbracoMediaAudio | UmbracoMediaVectorGraphics | UmbracoMediaVideo | VideoRow | VideoRowSettings | VisibilityControls | XmlSitemap;

/** Used to get typed properties on a block list property for the settings property */
export type TypedBlockListSettingsProperties = Article | ArticleControls | ArticleList | Author | AuthorList | CategoryList | CodeSnippetRow | CodeSnippetRowSettings | Contact | ContactFormControls | Content | ContentControls | EmptyPropertyType | Error | File | FooterControls | HeaderControls | HideProperty | Home | IconLinkRow | IconLinkRowSettings | Image | ImageCarouselRow | ImageCarouselRowSettings | ImageRow | ImageRowSettings | LatestArticlesRow | LatestArticlesRowSettings | MainImageControls | Member | RichTextRow | RichTextRowSettings | SeoControls | Search | SpacingProperties | UmbracoMediaArticle | UmbracoMediaAudio | UmbracoMediaVectorGraphics | UmbracoMediaVideo | VideoRow | VideoRowSettings | VisibilityControls | XmlSitemap;

/** Used to get typed properties on a nested content property */
export type TypedNestedContentProperties = Article | ArticleControls | ArticleList | Author | AuthorList | CategoryList | CodeSnippetRow | CodeSnippetRowSettings | Contact | ContactFormControls | Content | ContentControls | EmptyPropertyType | Error | File | FooterControls | HeaderControls | HideProperty | Home | IconLinkRow | IconLinkRowSettings | Image | ImageCarouselRow | ImageCarouselRowSettings | ImageRow | ImageRowSettings | LatestArticlesRow | LatestArticlesRowSettings | MainImageControls | Member | RichTextRow | RichTextRowSettings | SeoControls | Search | SpacingProperties | UmbracoMediaArticle | UmbracoMediaAudio | UmbracoMediaVectorGraphics | UmbracoMediaVideo | VideoRow | VideoRowSettings | VisibilityControls | XmlSitemap;

/** Used to get typed properties on a model - For nested properties use TypedBlockListContentProperties, TypedBlockListSettingsProperties */
export type TypedProperties = Article | ArticleControls | ArticleList | Author | AuthorList | CategoryList | CodeSnippetRow | CodeSnippetRowSettings | Contact | ContactFormControls | Content | ContentControls | EmptyPropertyType | Error | File | FooterControls | HeaderControls | HideProperty | Home | IconLinkRow | IconLinkRowSettings | Image | ImageCarouselRow | ImageCarouselRowSettings | ImageRow | ImageRowSettings | LatestArticlesRow | LatestArticlesRowSettings | MainImageControls | Member | RichTextRow | RichTextRowSettings | SeoControls | Search | SpacingProperties | UmbracoMediaArticle | UmbracoMediaAudio | UmbracoMediaVectorGraphics | UmbracoMediaVideo | VideoRow | VideoRowSettings | VisibilityControls | XmlSitemap;

export type UmbracoMediaArticle = IUmbracoMediaArticle & {
  __typename?: 'UmbracoMediaArticle';
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type UmbracoMediaAudio = IUmbracoMediaAudio & {
  __typename?: 'UmbracoMediaAudio';
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type UmbracoMediaVectorGraphics = IUmbracoMediaVectorGraphics & {
  __typename?: 'UmbracoMediaVectorGraphics';
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

export type UmbracoMediaVideo = IUmbracoMediaVideo & {
  __typename?: 'UmbracoMediaVideo';
  /** in bytes */
  umbracoBytes?: Maybe<Label>;
  umbracoExtension?: Maybe<Label>;
  umbracoFile?: Maybe<DefaultProperty>;
};

/** Represents an unsupported property value. */
export type UnsupportedProperty = PropertyValue & {
  __typename?: 'UnsupportedProperty';
  /** Gets the message of the property. */
  message: Scalars['String']['output'];
  /** The model of the property value */
  model: Scalars['String']['output'];
};

export enum UrlMode {
  Absolute = 'ABSOLUTE',
  Auto = 'AUTO',
  Default = 'DEFAULT',
  Relative = 'RELATIVE'
}

export type VideoRow = IVideoRow & {
  __typename?: 'VideoRow';
  /** Add a caption to display under the video if you would like */
  caption?: Maybe<DefaultProperty>;
  /** Add the YouTube Url in here */
  videoUrl?: Maybe<DefaultProperty>;
};

export type VideoRowSettings = IHideProperty & ISpacingProperties & IVideoRowSettings & {
  __typename?: 'VideoRowSettings';
  /** Set this to true if you want to hide this row from the front end of the site */
  hide?: Maybe<DefaultProperty>;
  marginBottom?: Maybe<DefaultProperty>;
  marginLeft?: Maybe<DefaultProperty>;
  marginRight?: Maybe<DefaultProperty>;
  marginTop?: Maybe<DefaultProperty>;
  paddingBottom?: Maybe<DefaultProperty>;
  paddingLeft?: Maybe<DefaultProperty>;
  paddingRight?: Maybe<DefaultProperty>;
  paddingTop?: Maybe<DefaultProperty>;
};

export type VisibilityControls = IVisibilityControls & {
  __typename?: 'VisibilityControls';
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type XmlSitemap = IVisibilityControls & IxmlSitemap & {
  __typename?: 'XMLSitemap';
  hideFromTopNavigation?: Maybe<DefaultProperty>;
  /** Tick this if you want to hide this page from the XML sitemap */
  hideFromXMLSitemap?: Maybe<DefaultProperty>;
  /** Tick this box if you want to hide this page from the navigation and from search results */
  umbracoNaviHide?: Maybe<DefaultProperty>;
};

export type ContentTypeQueryQueryVariables = Exact<{
  baseUrl: Scalars['String']['input'];
  route: Scalars['String']['input'];
  culture?: InputMaybe<Scalars['String']['input']>;
  includePreview: Scalars['Boolean']['input'];
}>;


export type ContentTypeQueryQuery = { __typename?: 'Query', contentByRoute?: { __typename?: 'ContentItem', name?: string | null, url?: string | null, properties: { __typename: 'Article' } | { __typename: 'ArticleControls' } | { __typename: 'ArticleList' } | { __typename: 'Author' } | { __typename: 'AuthorList' } | { __typename: 'CategoryList' } | { __typename: 'CodeSnippetRow' } | { __typename: 'CodeSnippetRowSettings' } | { __typename: 'Contact' } | { __typename: 'ContactFormControls' } | { __typename: 'Content' } | { __typename: 'ContentControls' } | { __typename: 'EmptyPropertyType' } | { __typename: 'Error' } | { __typename: 'File' } | { __typename: 'FooterControls' } | { __typename: 'HeaderControls' } | { __typename: 'HideProperty' } | { __typename: 'Home' } | { __typename: 'IconLinkRow' } | { __typename: 'IconLinkRowSettings' } | { __typename: 'Image' } | { __typename: 'ImageCarouselRow' } | { __typename: 'ImageCarouselRowSettings' } | { __typename: 'ImageRow' } | { __typename: 'ImageRowSettings' } | { __typename: 'LatestArticlesRow' } | { __typename: 'LatestArticlesRowSettings' } | { __typename: 'MainImageControls' } | { __typename: 'Member' } | { __typename: 'RichTextRow' } | { __typename: 'RichTextRowSettings' } | { __typename: 'SEOControls' } | { __typename: 'Search' } | { __typename: 'SpacingProperties' } | { __typename: 'UmbracoMediaArticle' } | { __typename: 'UmbracoMediaAudio' } | { __typename: 'UmbracoMediaVectorGraphics' } | { __typename: 'UmbracoMediaVideo' } | { __typename: 'VideoRow' } | { __typename: 'VideoRowSettings' } | { __typename: 'VisibilityControls' } | { __typename: 'XMLSitemap' } } | null };

export type CreateTokenMutationVariables = Exact<{
  scope?: InputMaybe<Scalars['Any']['input']>;
}>;


export type CreateTokenMutation = { __typename?: 'Mutation', createToken: { __typename?: 'JwtToken', expires: any, header: string, prefix: string, token: string } };


export const ContentTypeQueryDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"contentTypeQuery"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"baseUrl"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}}},{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"route"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}}},{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"culture"}},"type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}},{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"includePreview"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"Boolean"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"contentByRoute"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"baseUrl"},"value":{"kind":"Variable","name":{"kind":"Name","value":"baseUrl"}}},{"kind":"Argument","name":{"kind":"Name","value":"route"},"value":{"kind":"Variable","name":{"kind":"Name","value":"route"}}},{"kind":"Argument","name":{"kind":"Name","value":"inContext"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"culture"},"value":{"kind":"Variable","name":{"kind":"Name","value":"culture"}}},{"kind":"ObjectField","name":{"kind":"Name","value":"includePreview"},"value":{"kind":"Variable","name":{"kind":"Name","value":"includePreview"}}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"url"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"urlMode"},"value":{"kind":"EnumValue","value":"ABSOLUTE"}}]},{"kind":"Field","name":{"kind":"Name","value":"properties"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}}]}}]}}]}}]} as unknown as DocumentNode<ContentTypeQueryQuery, ContentTypeQueryQueryVariables>;
export const CreateTokenDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"createToken"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"scope"}},"type":{"kind":"NamedType","name":{"kind":"Name","value":"Any"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"createToken"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"claims"},"value":{"kind":"ListValue","values":[{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"name"},"value":{"kind":"StringValue","value":"headless-scope","block":false}},{"kind":"ObjectField","name":{"kind":"Name","value":"value"},"value":{"kind":"Variable","name":{"kind":"Name","value":"scope"}}}]}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"expires"}},{"kind":"Field","name":{"kind":"Name","value":"header"}},{"kind":"Field","name":{"kind":"Name","value":"prefix"}},{"kind":"Field","name":{"kind":"Name","value":"token"}}]}}]}}]} as unknown as DocumentNode<CreateTokenMutation, CreateTokenMutationVariables>;