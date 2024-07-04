import { graphql } from '@/gql';
import { CreateTokenMutation } from '@/gql/graphql';
import { Client, cacheExchange, fetchExchange } from '@urql/core';
import { authExchange } from '@urql/exchange-auth';

const GRAPHQL_ENDPOINT = 'https://localhost:44368/graphql/';

const API_KEY = 'qLV$6eo5*2OBX9yGGz*BiQVnGlr778nDmy!GX60A@JwL1Ql&AFQRkru!#zW9XVTqF2zzc1O7Q4XIcwuMZDUDNrsfdy3gw5Ey7P@';

type UHeadlessScopes =
    | 'content.by.route.query'
    | 'global.content.read'
    | 'property.values.member.picker'
    | 'global.member.read';

const isTokenExpired = (token: Awaited<ReturnType<typeof createToken>>) => {
    return Date.now() > token.expires * 1000;
};

const createTokenQuery = graphql(/* GraphQL */ `
    mutation createToken($scope: Any) {
        createToken(claims: [{ name: "headless-scope", value: $scope }]) {
            expires
            header
            prefix
            token
        }
    }
`);

const tokenClient = new Client({
    url: GRAPHQL_ENDPOINT,
    exchanges: [fetchExchange],
    fetchOptions: {
        headers: {
            'X-UHeadless-Api-Key': API_KEY,
        },
    },
});

const tokenMap = new Map<string, CreateTokenMutation>();

const createToken = async (scope: UHeadlessScopes[]) => {
    const tokenKey = scope.join(',');
    if (tokenMap.has(tokenKey)) {
        const token = tokenMap.get(tokenKey);

        if (token) {
            return token.createToken;
        }

        throw new Error('Failed to create token. API responed with no data');
    }

    const { data, error } = await tokenClient.mutation(createTokenQuery, { scope });

    if (error) {
        console.error(error);
        throw new Error('Failed to create token. API responed with:', error);
    }

    if (!data) {
        throw new Error('Failed to create token. API responed with no data');
    }

    tokenMap.set(tokenKey, data);

    return data.createToken;
};

const createClient = (scope: UHeadlessScopes[]) =>
    new Client({
        url: GRAPHQL_ENDPOINT,
        exchanges: [
            authExchange(async (utils) => {
                const token = await createToken(scope);
                
                return {
                    addAuthToOperation(operation) {
                        return utils.appendHeaders(operation, {
                            [token.header]: token.prefix + token.token,
                        });
                    },
                    willAuthError(_operation) {
                        return isTokenExpired(token);
                    },
                    didAuthError(error, _operation) {
                        return error.graphQLErrors.some((e) => e.message === 'Unauthorized');
                    },
                    async refreshAuth() {
                        await createToken(scope);
                    },
                };
            }),
            fetchExchange,
        ],
    });

export { createClient };
