import { graphql } from '@/gql';
import { createClient } from '@/lib/uheadless/client';
import Link from 'next/link';

const contentTypeQuery = graphql(/* GraphQL */ `
    query contentTypeQuery($baseUrl: String!, $route: String!, $culture: String, $includePreview: Boolean!) {
        contentByRoute(
            baseUrl: $baseUrl
            route: $route
            inContext: { culture: $culture, includePreview: $includePreview }
        ) {
            name
            url(urlMode: ABSOLUTE)
            properties {
                __typename
            }
            children {
                name
                url(urlMode: RELATIVE)
            }
        }
    }
`);

const homePagePropertiesQuery = graphql(/* GraphQL */ `
    query homePagePropertiesQuery($baseUrl: String!, $route: String!, $culture: String, $includePreview: Boolean!) {
        contentByRoute(
            baseUrl: $baseUrl
            route: $route
            inContext: { culture: $culture, includePreview: $includePreview }
        ) {
            properties {
                ... on IHome {
                    mainImage {
                        mediaItems {
                            url(urlMode: ABSOLUTE)
                        }
                    }
                    title {
                        value
                    }
                    subtitle {
                        value
                    }
                    socialIconLinks {
                        ...SocialIconLinks
                    }
                    contentRows {
                        ...ContentRows
                    }
                    metaName {
                        value
                    }
                    metaDescription {
                        value
                    }
                    metaKeywords {
                        value
                    }
                }
                __typename
            }
        }
    }

    fragment SocialIconLinks on BlockList {
        blocks {
            contentProperties {
                ... on IIconLinkRow {
                    icon {
                        mediaItems {
                            url(urlMode: ABSOLUTE)
                        }
                    }
                    link {
                        links {
                            url(urlMode: RELATIVE)
                            target
                            name
                            type
                        }
                    }
                }
                __typename
            }
            settingsProperties {
                ... on IHideProperty {
                    hide {
                        value
                    }
                }
                __typename
            }
        }
    }

    fragment ContentRows on BlockList {
        blocks {
            contentProperties {
                ... on IRichTextRow {
                    content {
                        value
                    }
                }
                ... on IImageRow {
                    image {
                        mediaItems {
                            url(urlMode: ABSOLUTE)
                        }
                    }
                    caption {
                        value
                    }
                }
                ... on IVideoRow {
                    videoUrl {
                        value
                    }
                    caption {
                        value
                    }
                }
                ... on ICodeSnippetRow {
                    title {
                        value
                    }
                    code {
                        value
                    }
                }
                ... on ImageCarouselRow {
                    images {
                        mediaItems {
                            url(urlMode: ABSOLUTE)
                        }
                    }
                }
                ... on LatestArticlesRow {
                    articleList {
                        items {
                            url(urlMode: ABSOLUTE)
                            name
                        }
                    }
                }
                __typename
            }
            settingsProperties {
                ... on ISpacingProperties {
                    paddingTop {
                        value
                    }
                    paddingBottom {
                        value
                    }
                    paddingLeft {
                        value
                    }
                    paddingRight {
                        value
                    }
                    marginTop {
                        value
                    }
                    marginBottom {
                        value
                    }
                    marginLeft {
                        value
                    }
                    marginRight {
                        value
                    }
                }
                ... on IHideProperty {
                    hide {
                        value
                    }
                }
                __typename
            }
        }
    }
`);

export default async function Home({ params }: { params: { slug: string[] } }) {
    const client = createClient(['content.by.route.query']);

    const route = '/' + (params.slug?.join('/') || '');
    const { data: contentType, error } = await client.query(contentTypeQuery, {
        baseUrl: '',
        route,
        culture: null,
        includePreview: false,
    });

    let properties;
    if (contentType?.contentByRoute?.properties?.__typename === 'Home') {
        const { data: homePageProperties, error: homePagePropertiesError } = await client.query(
            homePagePropertiesQuery,
            {
                baseUrl: '',
                route,
                culture: null,
                includePreview: false,
            },
        );

        properties = (
            <div className="flex justify-center items-start flex-row gap-40">
                <p>PROPERTIES</p>
                <span>
                    DATA: <pre>{JSON.stringify(homePageProperties, undefined, 2)}</pre>
                </span>
                <span>
                    ERROR: <pre>{JSON.stringify(homePagePropertiesError, undefined, 2)}</pre>
                </span>
            </div>
        );
    }

    return (
        <main className="flex flex-col justify-center items-center my-10">
            <p className="mb-10">CHILDREN</p>
            <div className="flex gap-10 max-w-screen-lg flex-wrap">
                {contentType?.contentByRoute?.children
                    ?.filter((child) => child != null)
                    .map((child) => (
                        <Link
                            key={child.url}
                            href={child.url ?? ''}
                            className="bg-slate-600 hover:bg-slate-800 p-4 px-6 rounded"
                        >
                            <p>{child.name}</p>
                        </Link>
                    ))}
            </div>
            <div className="flex min-h-screen flex-col items-center justify-start gap-40 p-24">
                <div className="flex justify-center items-start flex-row gap-40">
                    <p>ROUTE: {route}</p>
                    <span>
                        DATA: <pre>{JSON.stringify(contentType, undefined, 2)}</pre>
                    </span>
                    <span>
                        ERROR: <pre>{JSON.stringify(error, undefined, 2)}</pre>
                    </span>
                </div>
            </div>
            {properties}
        </main>
    );
}
