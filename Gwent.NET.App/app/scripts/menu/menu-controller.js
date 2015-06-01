(function () {
    'use strict';

    angular.module('app.menu')
        .controller('MenuController', function ($scope, $log, $state, backendService, userService) {
            var ctrl = this;
            var data = $scope.data = {
                user: {},
                login: {},
                register: {}
            };
            var methods = $scope.methods = {};
            var setUser = function (user) {
                data.user = user;
                if (user !== null && !$state.includes('menu.main')) {
                    $state.go('menu.main');
                }
            };
            methods.login = function () {
                userService.login(data.login.username, data.login.password)
                    .then(function (user) {
                        $log.info('Login successful');
                        setUser(user);
                        data.login.error = '';
                    },
                    function (msg) {
                        data.login.error = 'Login failed';
                    });
            };
            methods.logout = function () {
                userService.logout().then(function () {
                    $log.info('Logout successful');
                    setUser(null);
                }, function (msg) {
                    $log.error('Logout failed');
                    data.login.error = 'Logout failed';
                    setUser(null);
                });
            };
            methods.register = function () {
                if (data.register.password !== data.register.passwordAgain) {
                    data.register.error = 'Password does not match';
                    return;
                }
                userService.register(data.register.username, data.register.password)
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

            setUser(userService.getUser());
        });
})();