angular.module('message-service', [])
    .service('MessageService', function () {

        this.send = function(){
            alert('Hello world');
        }
    });