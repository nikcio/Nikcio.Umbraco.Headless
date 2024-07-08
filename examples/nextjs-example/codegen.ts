import { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
    schema: 'https://localhost:44368/graphql/',
    documents: ['./**/*.tsx', './**/*.ts'],
    ignoreNoDocuments: true,
    generates: {
        './gql/': {
            preset: 'client',
            plugins: [],
        },
    },
    watch: true
    
};

export default config;
