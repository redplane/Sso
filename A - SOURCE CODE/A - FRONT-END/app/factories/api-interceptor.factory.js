"use strict";

angular.module('api-interceptor', [])
    .factory('apiInterceptor', ['$injector', function ($injector) {


        return {
            /*
            * Callback which is fired when request is made.
            * */
            request: function (x) {
                let url = x.url;

                //toastr.success(url);
                if (url.indexOf('/api/') === -1)
                    return x;

                return x;
            },

            /*
            * Callback which is fired when request is made failingly.
            * */
            requestError: function (config) {
                return config;
            },

            /*
            * Callback which is fired when response is sent back from back-end.
            * */
            response: function (x) {

                return x;
            },

            /*
            * Callback which is fired when response is failed.
            * */
            responseError: function (x) {
                return x;
            }
        }
    }]);