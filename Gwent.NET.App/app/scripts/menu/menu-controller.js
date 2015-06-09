(function () {
    'use strict';

    angular.module('app.menu')
        .controller('MenuController', function ($scope, $log, $state, backendService, userService) {
            var data = $scope.data = {
                user: null,
                login: {},
                register: {}
            };
            var methods = $scope.methods = {};
            var setUser = function (user) {
                data.user = user;
            };

            methods.login = function () {
                userService.login(data.login.username, data.login.password)
                    .then(function (user) {
                        $log.info('Login successful');
                        setUser(user);
                        data.login.error = '';
                        data.login.username = '';
                        data.login.password = '';
                        $state.go('menu.home');
                    },
                    function () {
                        data.login.error = 'Login failed';
                        data.login.password = '';
                    });
            };
            methods.logout = function () {
                userService.logout().then(function () {
                    $log.info('Logout successful');
                    data.error = '';
                    setUser(null);
                }, function () {
                    $log.error('Logout failed');
                    data.error = 'Logout failed';
                    setUser(null);
                });
            };
            methods.register = function () {
                var username = data.register.username;
                var password = data.register.password;

                if (data.register.password !== data.register.passwordAgain) {
                    data.register.error = 'Password does not match';
                    return;
                }
                userService.register(username, password)
                    .then(function () {
                        $log.info('Registration successful');
                        data.register.error = '';
                        data.register.username = '';
                        data.register.password = '';
                        data.register.passwordAgain = '';
                        data.login.username = username;
                        data.login.password = password;
                        methods.login();
                    },
                    function () {
                        data.register.error = 'Registration failed';
                        data.register.password = '';
                        data.register.passwordAgain = '';
                    });
            };
            $scope.$on('userChanged', function (event, user) {
                setUser(user);
            });

            setUser(userService.getUser());
        });
})();