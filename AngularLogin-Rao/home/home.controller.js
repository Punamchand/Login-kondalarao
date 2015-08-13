(function () {
    'use strict';

    angular
        .module('app')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$rootScope','$location'];
    function HomeController($rootScope,$location) {
        var vm = this;
        vm.user = $rootScope.globals.currentUser.username;

        if($rootScope.globals.currentUser.remember == false) {

            var curdate = new Date();
            var date2 = new Date($rootScope.globals.currentUser.logindate);
            var timeDiff = Math.abs(date2.getTime() - curdate.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

            if (diffDays > 3) {
                $location.path('/login');
            }
        }
    }


})();