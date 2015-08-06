(function () {
    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = ['AuthenticationService', '$location', '$rootScope', 'FlashService'];
    function RegisterController(AuthenticationService, $location, $rootScope, FlashService) {
        var vm = this;

        vm.register = register;

        function register() {
            vm.dataLoading = true;
            AuthenticationService.Create(vm.user, function (response){
                    if (response.indexOf('Success') > -1) {
                        FlashService.Success('Registration successful', true);
                        $location.path('/login');
                    } else {
                        FlashService.Error(response);
                        vm.dataLoading = false;
                    }
                });
        }
    }

})();
