'use strict';

angular.module('sidebar', ['ngRoute'])
    .directive('sidebar', function () {
        return {
            restrict: 'E',
            templateUrl: "components/sidebar/sidebar.html",
            controller: 'SidebarController',
            scope: {
                items: '='
            }
        };
    })
    .controller('SidebarController', ['$scope', '$location', '$translate', 'categoryService',
        function ($scope, $location, $translate, categoryService) {

            //#region Properties

            /*
            * List of categories.
            * */
            $scope.categories = [];

            //#endregion

            //#region Methods

            /*
            * Callback which is fired when component has been initialized.
            * */
            $scope.init = function () {

                // Search categories condition.
                let options = {};

                // Start searching.
                categoryService.getCategories(options)
                    .then(function(x){
                        let data = x.data;
                        $scope.categories = data.records;
                    })
                    .catch(function(x){
                        console.log(x);
                    });
            };

        }]);