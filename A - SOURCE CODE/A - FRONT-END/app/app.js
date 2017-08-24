'use strict';

// Declare app level module which depends on views, and components
angular
    .module('api-documentation', [
        'ngRoute',
        'pascalprecht.translate',

        // Components.
        'navigation-bar',
        'sidebar',
        'personal-summary',
        'personal-technical-skill',
        'project-summary',
        'login',

        // Services.
        'message-service',
        'api-setting-service',
        'general-info-service',

        // 3rd libraries.
        'ngAnimate',
        'toastr',

        // Providers.
        'api-interceptor'

    ])
    .config(['$locationProvider', '$routeProvider', '$translateProvider', '$httpProvider',
        function ($locationProvider, $routeProvider, $translateProvider, $httpProvider) {

            // Interceptor registration.
            $httpProvider.interceptors.push('apiInterceptor');

            // Use static files loader.
            $translateProvider.useStaticFilesLoader({
                prefix: 'assets/data/language/locale-',
                suffix: '.json'
            });

            // Url hash configuration
            $locationProvider
                .hashPrefix('!');

            // Fallback url.
            $routeProvider.otherwise({redirectTo: '/login'});

        }])
    .controller('ApiDocumentationController', ['GeneralInfoService', '$scope',
        function (GeneralInfoService, $scope) {
        }]);
