'use strict';

angular.module('main-content', [
    'ngRoute'
])
    .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
        $locationProvider.hashPrefix('!');

        $routeProvider
            .when('/', {
                controller: "MainContentController",
                templateUrl: "components/main-content/main-content.component.html"
            });
    }])
    .controller('MainContentController', ['$scope',
        function ($scope) {

            //#region Properties

            // Collection of slides.
            $scope.slides = [];

            // Hot images.
            $scope.hotImages = [];

            // Pagination information.
            $scope.pagination = {
                page: 1,
                records: 30,
                total: 3000
            };

            //#endregion

            //#region Methods

            /*
            * Event which is called when component has been initialized.
            * */
            $scope.init = function () {

                for (let index = 0; index < 10; index++) {
                    $scope.slides.push({
                        image: 'http://placehold.it/900x350',
                        text: 'Nice image',
                        id: $scope.slides.length
                    });
                }

                for (let index = 0; index < 20; index++){
                    $scope.hotImages.push({
                        caption: 'A beautiful image',
                        url: 'http://via.placeholder.com/300x200'
                    });
                }
            };

            /*
            * Raised when a category is selected in sidebar.
            * */
            $scope.selectCategory = function(category){
                console.log(category);
            };

            //#endregion
        }]);