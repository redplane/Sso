'use strict';

angular.module('personal-summary', [
    'ngRoute',
    'general-info-service'
])
    .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
        $locationProvider.hashPrefix('!');

        $routeProvider
            .when('/personal-summary', {
                controller: "PersonalSummaryController",
                templateUrl: "components/personal-summary/personal-summary.html",
                resolve: {
                    'information': ['GeneralInfoService', function (GeneralInfoService) {

                        // Initiate promises list.
                        let promises = [];
                        promises.push(GeneralInfoService.getObjectives());
                        promises.push(GeneralInfoService.getSummaries());
                        promises.push(GeneralInfoService.getCertificates());

                        // Resolve all promises to display information.
                        return Promise.all(promises)
                            .then(function success(results) {
                                return {
                                    objectives: results[0].data,
                                    summaries: results[1].data,
                                    certificates: results[2].data
                                }
                            });
                    }]
                }
            });
    }])
    .controller('PersonalSummaryController', ['GeneralInfoService', '$scope', 'information',
        function (GeneralInfoService, $scope, information) {

            //#region Properties

            // Personal objectives.
            $scope.objectives = information.objectives;

            // Personal summaries.
            $scope.summaries = information.summaries;

            // Personal certificates.
            $scope.certificates = information.certificates;

            //#endregion
        }]);