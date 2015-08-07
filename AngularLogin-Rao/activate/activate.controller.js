(function () {
    'use strict';

    angular
        .module('app')
        .controller('ActivateController', ActivateController);

    ActivateController.$inject = ['$rootScope','$location','AuthenticationService'];
    function ActivateController($rootScope,$location,AuthenticationService) {

        var vm = this;
        //vm.user = $rootScope.globals.currentUser.username;

        var userid =  $location.path().split(":")[1];
        AuthenticationService.ActivateUser(userid);
    }

})();