import { graphql } from '@/gql';
import { createClient } from '@/lib/uheadless/client';

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
        }
    }
`);

export default async function Home({ params }: { params: { slug: string[] }}) {
    const client = createClient(['content.by.route.query']);

    const route = '/' + (params.slug?.join('/') || '');
    const { data: contentType, error } = await client.query(contentTypeQuery, {
        baseUrl: '',
        route,
        culture: null,
        includePreview: false,
    });

    return (
        <main className="flex min-h-screen flex-col items-center justify-start gap-40 p-24">
            <p>ROUTE: {route}</p>
            <div className='flex justify-center items-start flex-row gap-40'>
            <p>DATA: <pre>{JSON.stringify(contentType, undefined, 2)}</pre></p>
            <p>ERROR: <pre>{JSON.stringify(error, undefined, 2)}</pre></p>
            </div>
        </main>
    );
}
