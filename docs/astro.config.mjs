import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import starlightLinksValidator from 'starlight-links-validator'

// https://astro.build/config
export default defineConfig({
  site: 'https://nikcio.github.io',
  base: '/Nikcio.UHeadless/',
  integrations: [
    starlight({
      title: 'Nikcio.UHeadless',
      social: {
        github: 'https://github.com/nikcio/nikcio.uheadless/',
      },
      editLink: {
        baseUrl: 'https://github.com/nikcio/Nikcio.UHeadless/tree/'
      },
      plugins: [starlightLinksValidator({
          errorOnRelativeLinks: false,
        })],
      sidebar: [
        {
          label: 'Welcome',
          items: [
            { label: 'Overview', link: '/overview/' },
            { label: 'Version 5', link: '/v5/start/' },
          ],
        },
        {
          label: 'Fundamentals',
          items: [
            { label: 'Getting started', link: 'v5/fundamentals/getting-started/' },
            { label: 'Security', link: 'v5/fundamentals/security/' },
            {
              label: 'Querying',
              collapsed: true,
              items: [
                { label: 'Content', link: 'v5/fundamentals/querying/content/' },
                { label: 'Media', link: 'v5/fundamentals/querying/media/' },
                { label: 'Members', link: 'v5/fundamentals/querying/members/' },
                { label: 'Properties', link: 'v5/fundamentals/querying/properties/' }
              ]
            },
          ]
        },
        {
          label: 'Extending',
          items: [
            { label: 'Content', link: 'v5/extending/content/' },
            { label: 'Media', link: 'v5/extending/media/' },
            { label: 'Member', link: 'v5/extending/member/' },
            {
              label: 'Properties',
              collapsed: true,
              items: [
                { label: 'Overview', link: 'v5/extending/properties/overview/' },
                { label: 'Rich text', link: 'v5/extending/properties/rich-text/' },
                { label: 'Media picker', link: 'v5/extending/properties/media-picker/' },
                { label: 'Block list', link: 'v5/extending/properties/block-list/' },
                { label: 'Custom editor', link: 'v5/extending/properties/custom-editor/' },
              ]
            },
          ]
        },
        {
          label: 'Reference',
          items: [
            { label: 'Options', link: 'v5/reference/options/' },
            { label: 'Endpoint options', link: 'v5/reference/endpoint-options/' },
            { label: 'Content', link: 'v5/reference/content/' },
            { label: 'Media', link: 'v5/reference/media/' },
            { label: 'Member', link: 'v5/reference/member/' },
            { label: 'Member', link: 'v5/reference/properties/' },
          ]
        },
        {
          label: 'Older versions',
          collapsed: true,
          items: [
            {
              label: 'Version 4',
              collapsed: true,
              items: [
                { label: 'Overview', link: '/v4/start/' },
                {
                  label: 'Fundamentals',
                  items: [
                    { label: 'Getting started', link: 'v4/fundamentals/getting-started/' },
                    { label: 'Security', link: 'v4/fundamentals/security/' },
                    { label: 'Extend UHeadless', link: 'v4/fundamentals/extend-uheadless/' },
                    {
                      label: 'Querying',
                      collapsed: true,
                      items: [
                        { label: 'Content', link: 'v4/fundamentals/querying/content/' },
                        { label: 'Media', link: 'v4/fundamentals/querying/media/' },
                        { label: 'Members', link: 'v4/fundamentals/querying/members/' },
                        { label: 'Properties', link: 'v4/fundamentals/querying/properties/' }
                      ]
                    },
                  ]
                },
                {
                  label: 'Extending',
                  items: [
                    { label: 'Content', link: 'v4/extending/content/' },
                    { label: 'Media', link: 'v4/extending/media/' },
                    { label: 'Member', link: 'v4/extending/member/' },
                    {
                      label: 'Properties',
                      collapsed: true,
                      items: [
                        { label: 'Overview', link: 'v4/extending/properties/overview/' },
                        { label: 'Rich text', link: 'v4/extending/properties/rich-text/' },
                        { label: 'Media picker', link: 'v4/extending/properties/media-picker/' },
                        { label: 'Block list', link: 'v4/extending/properties/block-list/' },
                        { label: 'Custom editor', link: 'v4/extending/properties/custom-editor/' },
                      ]
                    },
                  ]
                },
                {
                  label: 'Reference',
                  items: [
                    { label: 'Options', link: 'v4/reference/options/' },
                    { label: 'Endpoint options', link: 'v4/reference/endpoint-options/' },
                    { label: 'Paging options', link: 'v4/reference/paging-options/' },
                    {
                      label: 'Content',
                      collapsed: true,
                      items: [
                        { label: 'Content Reference', link: 'v4/reference/content/overview/' },
                        { label: 'Content Bases', link: 'v4/reference/content/bases/' },
                        { label: 'Content Basics', link: 'v4/reference/content/basics/' }
                      ]
                    },
                    {
                      label: 'Media',
                      collapsed: true,
                      items: [
                        { label: 'Media Reference', link: 'v4/reference/media/overview/' },
                        { label: 'Media Bases', link: 'v4/reference/media/bases/' },
                        { label: 'Media Basics', link: 'v4/reference/media/basics/' }
                      ]
                    },
                    {
                      label: 'Members',
                      collapsed: true,
                      items: [
                        { label: 'Members Reference', link: 'v4/reference/members/overview/' },
                        { label: 'Members Bases', link: 'v4/reference/members/bases/' },
                        { label: 'Members Basics', link: 'v4/reference/members/basics/' }
                      ]
                    },
                    {
                      label: 'Properties',
                      collapsed: true,
                      items: [
                        { label: 'Properties Reference', link: 'v4/reference/properties/overview/' },
                        { label: 'Properties Bases', link: 'v4/reference/properties/bases/' },
                        { label: 'Properties Basics', link: 'v4/reference/properties/basics/' }
                      ]
                    },
                  ]
                }
              ]
            },
            {
              label: 'Version 3',
              collapsed: true,
              items: [
                { label: 'Overview', link: '/v3/start/' },
                { label: 'Installation', link: '/v3/installation/' },
                { label: 'Options', link: '/v3/options/' },
                {
                  label: 'Content',
                  items: [
                    { label: 'Bases', link: '/v3/content/bases/' },
                    { label: 'Basics', link: '/v3/content/basics/' },
                    { label: 'Extend existing content', link: '/v3/content/extendexisting/' },
                    { label: 'Extend existing content with public access settings', link: '/v3/content/extendexistingpublicaccess/' },
                  ]
                },
                {
                  label: 'Content queries',
                  items: [
                    { label: 'Get content by absoulte route', link: '/v3/contentqueries/getcontentbyabsoluteroute/' },
                    { label: 'Get content by id with variables', link: '/v3/contentqueries/getcontentbyidwithvariables/' },
                    { label: 'Get content by tag', link: '/v3/contentqueries/getcontentbytag/' },
                  ]
                },
                {
                  label: 'Other packages',
                  items: [
                    { label: 'How to use a extending package', link: '/v3/otherpackages/howtouseaextendingpackage/' },
                  ]
                },
                {
                  label: 'Property values',
                  items: [
                    { label: 'Bases', link: '/v3/propertyvalues/bases/' },
                    { label: 'Basics', link: '/v3/propertyvalues/basics/' },
                    { label: 'Extend block list', link: '/v3/propertyvalues/extendblocklist/' },
                    { label: 'Extend simple editor', link: '/v3/propertyvalues/extendexistingsimple/' },
                    { label: 'Extend media picker', link: '/v3/propertyvalues/extendmediapicker/' },
                    { label: 'Create new property value', link: '/v3/propertyvalues/newvalue/' },
                    { label: 'Unsupported editors', link: '/v3/propertyvalues/unsupported/' },
                  ]
                },
                {
                  label: 'Querying',
                  items: [
                    { label: 'Properties', link: '/v3/querying/properties/' },
                  ]
                },
                {
                  label: 'Server options',
                  items: [
                    { label: 'Disable UI', link: '/v3/serveroptions/disableui/' },
                  ]
                }
              ]
            },
            {
              label: 'Version 2',
              collapsed: true,
              items: [
                { label: 'Overview', link: '/v2/start/' },
                { label: 'Installation', link: '/v2/installation/' },
                { label: 'Options', link: '/v2/options/' },
                {
                  label: 'Content',
                  items: [
                    { label: 'Bases', link: '/v2/content/bases/' },
                    { label: 'Basics', link: '/v2/content/basics/' },
                    { label: 'Extend existing content', link: '/v2/content/extendexisting/' },
                  ]
                },
                {
                  label: 'Content queries',
                  items: [
                    { label: 'Get content by absoulte route', link: '/v2/contentqueries/getcontentbyabsoluteroute/' },
                  ]
                },
                {
                  label: 'Property values',
                  items: [
                    { label: 'Bases', link: '/v2/propertyvalues/bases/' },
                    { label: 'Basics', link: '/v2/propertyvalues/basics/' },
                    { label: 'Extend block list', link: '/v2/propertyvalues/extendblocklist/' },
                    { label: 'Extend simple editor', link: '/v2/propertyvalues/extendexistingsimple/' },
                    { label: 'Extend media picker', link: '/v2/propertyvalues/extendmediapicker/' },
                    { label: 'Create new property value', link: '/v2/propertyvalues/newvalue/' },
                  ]
                },
              ]
            },
            {
              label: 'Version 1',
              collapsed: true,
              items: [
                { label: 'Overview', link: '/v1/start/' },
                {
                  label: 'Content models',
                  items: [
                    { label: 'Edit content models', link: '/v1/contentmodels/editcontentmodels/' },
                  ]
                },
                {
                  label: 'Property values',
                  items: [
                    { label: 'Edit property values', link: '/v1/propertyvalues/editpropertyvalues/' },
                  ]
                }
              ]
            }
          ]
        }
      ],
    }),
  ],
});
