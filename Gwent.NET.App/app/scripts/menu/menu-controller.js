(function () {
    'use strict';

    angular.module('app.menu')
        .controller('MenuController', function ($scope, $log, $state, backendService, userService) {
            var ctrl = this;
            var data = $scope.data = {};
            var methods = $scope.methods = {};
            methods.login = function () {
                backendService.methods.login(data.login.username, data.login.password)
                    .then(function (user) {
                        userService.setUser(user);
                        $log.info('Login successful');
                        data.login.error = '';
                    },
                    function (msg) {
                        data.login.error = 'Login failed';
                    });
            };
            methods.logout = function () {

            };
            methods.register = function () {
                if (data.register.password !== data.register.passwordAgain) {
                    data.register.error = 'Password does not match';
                    return;
                }
                backendService.methods.register(data.register.username, data.register.password)
                    .then(function (response) {
                        $log.info('Registration successful');
                        data.register.error = '';
                    },
                    function (msg) {
                        data.register.error = 'Registration failed';
                        data.register.password = '';
                        data.register.passwordAgain = '';
                    });
            };

            $scope.$on('userChanged', function () {
                data.user = userService.user;
                if (data.user != null) {
                    $state.go('menu.main');
                }
            });
        });
})();