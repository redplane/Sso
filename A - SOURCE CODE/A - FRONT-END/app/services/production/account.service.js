angular.module('iClipServices')
    .service('accountService', ['baseUrl', 'urlsList','$http',
        function (baseUrl, urlsList, $http) {

        //#region Methods

        /*
        * Get requester personal profile.
        * */
        this.getPersonalProfile = function(){
            let url = baseUrl + '/' + urlsList.getPersonalProfile;
            return $http.get(url);
        };

        /*
        * Login into system using basic information.
        * */
        this.loginBasic = function(information){
            let url = baseUrl + '/' + urlsList.internalLogin;
            return $http.post(url, information);
        };

        /*
        * Login into system using oauth information.
        * */
        this.loginExternally = function(information){
            let url = baseUrl + '/' + urlsList.externalLogin;
            return $http.post(url, information);
        };

        /*
        * Register an account using basic information.
        * */
        this.registerBasic = function(information){
            let url = baseUrl + '/' + urlsList.basicProfileRegistration;
            return $http.post(url, information);
        };

        //#endregion
    }]);