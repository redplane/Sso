'use strict';

angular.module('main-content', [
    'ngRoute'
])
    .config(['$locationProvider', '$routeProvider',
        function ($locationProvider, $routeProvider) {
        $locationProvider.hashPrefix('!');

        $routeProvider
            .when('/', {
                controller: "MainContentController",
                templateUrl: "components/main-content/main-content.component.html"
            });
    }])
    .controller('MainContentController', ['$scope', 'clipService', 'clipThumbnailService', 'clipCategorizingService',
        function ($scope, clipService, clipThumbnailService, clipCategorizingService) {

            //#region Properties

            // Collection of slides.
            $scope.slides = [];

            // Pagination information.
            $scope.pagination = {
                page: 1,
                records: 30,
                total: 3000
            };

            /*
            * List of clips which should be displayed on the screen.
            * */
            $scope.clipsList = {
                records: [],
                total: 0
            };

            /*
            * List of clip thumbnails should be displayed on the screen.
            * */
            $scope.clipThumbnailsList = {
                records: [],
                total: 0
            };

            /*
            * Conditions which are for searching clips.
            * */
            $scope.findClipsCondition = {
                pagination:{
                    page: 1,
                    records: 30
                }
            };

            //#endregion

            //#region Methods

            /*
            * Event which is called when component has been initialized.
            * */
            $scope.init = function () {

                // Search for hot trend items.
                $scope.findClipsCondition = {};
                $scope.findClipThumbnailsCondition = {};

                $scope.findClips($scope.findClipsCondition);
                $scope.findClipThumbnails($scope.findClipThumbnailsCondition);

            };

            /*
            * Raised when a category is selected in sidebar.
            * */
            $scope.selectCategory = function(category){

                // Re-initiate condition.
                $scope.findClipsCondition = {
                    categoryIds: [category.id],
                    pagination: {
                        page: 1,
                        records: 20
                    }
                };

                $scope.findClips($scope.findClipsCondition);

            };

            /*
            * Find clips base on specific condition.
            * */
            $scope.findClips = function(condition){
                console.log(condition);
                clipCategorizingService.getClipCategorizings(condition)
                    .then(function(x){
                        let data = x.data;
                        if (!data)
                            return;

                        console.log(data);
                        $scope.clipsList = data;
                    })
                    .catch(function(x){
                        console.log(x);
                    });
            };

            /*
            * Find clip thumbnails by using specific conditions.
            * */
            $scope.findClipThumbnails = function(condition){
                clipThumbnailService.getClipThumbnails(condition)
                    .then(function(x){
                        let data = x.data;
                        if (!data)
                            return;
                        
                        $scope.clipThumbnailsList = data;
                        $scope.$applyAsync();
                    });
            };
            //#endregion
        }]);