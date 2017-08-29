/*
* Application services declaration.
* */
angular.module('iClipServices', [])
    .constant('urlsList', {

        // Service base url.
        baseUrl: 'http://xvrfkyt79uwhch3ee.stoplight-proxy.io',

        // Url which is for searching categories by using specific conditions.
        getCategories: 'api/category/search',

        // Url which is for initiating category.
        initCategory: 'api/category'
    });