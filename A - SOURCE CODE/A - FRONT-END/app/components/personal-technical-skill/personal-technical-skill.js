'use strict';

angular.module('personal-technical-skill', [
    'ngRoute',
    'general-info-service',
    'ui.bootstrap'
])
    .directive('personalTechnicalSkill', function () {
        return {
            restrict: "E"
        };
    })
    .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {

        $locationProvider.hashPrefix('!');
        $routeProvider
            .when('/technical-skill', {
                controller: "TechnicalSkillController",
                templateUrl: "components/personal-technical-skill/personal-technical-skill.html"
            });
    }])
    .controller('TechnicalSkillController', [
        '$scope', 'GeneralInfoService',
        function ($scope, GeneralInfoService) {
            $scope.skillSummary = {};

            // This callback is fired when component has been loaded successfully.
            $scope.init = function () {
                $scope.reloadSkillsSummary();
            };

            // Reload skills summary,
            $scope.reloadSkillsSummary = function () {

                // Load personal technical skills.
                GeneralInfoService.getTechnicalSkills()
                    .then(function (x) {
                        var data = x.data;
                        if (data == null)
                            return;

                        // Update skill summary.
                        $scope.skillSummary = data;
                    })
                    .catch(function (x) {
                    })
            }

            // Depend on skill point to decide class of progressbar.
            $scope.findProgressbarClass = function(skillPoint){
                if (skillPoint == null)
                    return "";

                if (skillPoint < 50)
                    return "danger";

                if (50 <= skillPoint && skillPoint <= 60)
                    return "warning";

                if (60 < skillPoint && skillPoint <= 80)
                    return "info";

                return "primary";
            }
    }]);