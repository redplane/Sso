'use strict';

angular.module('project-summary', [
    'ngRoute',
    'general-info-service',
    'ui.bootstrap'
])
    .directive('projectSummary', function () {
        return {
            restrict: "E"
        };
    })
    .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {

        $locationProvider.hashPrefix('!');
        $routeProvider
            .when('/project-summary', {
                controller: "ProjectSummaryController",
                templateUrl: "components/project-summary/project-summary.html"
            });
    }])
    .controller('ProjectSummaryController', [
        '$scope', 'GeneralInfoService',
        function ($scope, GeneralInfoService) {
            $scope.projects = {};

            // This callback is fired when component has been loaded successfully.
            $scope.init = function () {
                $scope.reloadProjects();
            };

            // Reload projects list.
            $scope.reloadProjects = function(){
                GeneralInfoService.getProjects()
                    .then(function(x) {

                       var data = x.data;
                       if (data == null)
                           return;

                       $scope.projects = data.projects;
                    })
                    .catch(function(x){

                    });
            };

            // Check whether url is valid or not. If the url is not valid, fallback url will be used.
            $scope.getLink = function(url){
                if (url == null || url.length < 1)
                    return "javascript:void(0);";

                return url;
            }
    }]);