(function () {
    'use strict';

    angular
        .module('app')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$location', 'AuthenticationService', 'FlashService'];
    function LoginController($location, AuthenticationService, FlashService) {
        var vm = this;
        //$rootScope.globals.currentUser.logindate = null;
        vm.login = login;

        (function initController() {
            // reset login status
            AuthenticationService.ClearCredentials();
        })();

        function login() {
            vm.dataLoading = true;
            AuthenticationService.Login(vm.emailid, vm.password, function (response) {
                if (response.indexOf('Success') > -1) {
                    var username = response.split("-")[1].replace('"','');
                    AuthenticationService.SetCredentials(vm.remember, vm.emailid,username, vm.password);
                    $location.path('/');

                } else {
                    FlashService.Error(response);
                    vm.dataLoading = false;
                }
            });
        };
    }

})();
