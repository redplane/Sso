'use strict';

angular.module('sidebar', ['ngRoute', 'general-info-service'])
    .directive('sidebar', function () {
        return {
            restrict: 'E',
            templateUrl: "components/sidebar/sidebar.html",
            controller: 'SidebarController',
            scope: {
                accountProfileName: '@',
                accountProfileImage: '@',
                accountProfileTitle: '@',
                items: '='
            }
        };
    })
    .controller('SidebarController', ['$scope', '$location', '$translate', function ($scope, $location, $translate) {

            // List of supported language.
            $scope.languages = [
                {
                    name: "LANGUAGE_ENGLISH",
                    value: "en"
                },
                {
                    name: "LANGUAGE_VIETNAMESE",
                    value: "vi"
                }
            ];

            // Currently selected language.
            $scope.currentLanguage = {};


            $scope.changeLanguage = function(){
                $translate.use($scope.currentLanguage.value);
            };

            // Callback which is fired when component has been initialized successfully.
            $scope.init = function () {
                $scope.currentLanguage = $scope.languages[0];
                $translate.use($scope.currentLanguage.value);
            };

            $scope.clickSidebarItem = function (item) {
                console.log($scope.languages);
                $scope.clickDetails({item: item, i: item});
            };

            // Get location path.
            $scope.getLocation = function () {
                return $location.path();
            };

            // Check whether url is chosen or not.
            $scope.isUrlChosen = function (url) {
                if (url == null || url.length < 1)
                    return false;

                return url.indexOf($location.path()) == 0;
            };

            // Get current language of application.
            $scope.isCurrentLanguage = function () {
                return $translate.use();
            }
        }]);