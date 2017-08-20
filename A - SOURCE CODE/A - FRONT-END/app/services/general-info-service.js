'use strict';

angular.module("general-info-service", ['api-setting-service'])
    .service('GeneralInfoService', ['ApiSettingService', '$http', function (ApiSettingService, $http) {

        //#region Properties

        // Main personal objectives.
        let objectives = null;

        // Personal summaries.
        let summaries = null;

        // Personal certificates.
        let certificates = null;

        // Technical skills.
        let technicalSkills = null;

        // Experienced projects.
        let projects = null;

        //#endregion

        //#region Methods

        /*
        * Find personal missions which should be archived in future.
        * */
        this.getObjectives = function () {
            if (objectives != null) {
                return Promise.resolve(objectives);
            }

            let promise = $http.get(ApiSettingService.getPersonalObjectiveLink);
            promise.then(function (x) {
                objectives = x;
            });

            return promise;
        };

        /*
        * Find personal summary information.
        * */
        this.getSummaries = function () {

            // Summaries have been loaded before.
            if (summaries != null)
                return Promise.resolve(summaries);

            let promise = $http.get(ApiSettingService.getProfessionalSummaryLink);
            promise.then(function success(x){
               summaries = x;
            });

            return promise;
        };

        /*
        * Find list of certificates.
        * */
        this.getCertificates = function () {

            if (certificates != null)
                return Promise.resolve(certificates);

            let promise = $http.get(ApiSettingService.getCertificateLink);
            promise.then(function success(x){
               certificates = x;
            });

            return promise;
        };

        /*
        * Find personal technical skills.
        * */
        this.getTechnicalSkills = function () {
            if (technicalSkills != null)
                return Promise.resolve(technicalSkills);

            let promise = $http.get(ApiSettingService.getPersonalSkillsLink);
            promise.then(function success(x){
                technicalSkills = x;
            });

            return promise;
        };

        /*
        * Find personal projects.
        * */
        this.getProjects = function () {

            if (projects != null)
                return Promise.resolve(projects);

            let promise = $http.get(ApiSettingService.getProjectsLink);
            promise.then(function success(x){
                projects = x;
            });

            return promise;
        };

        //#endregion
    }]);