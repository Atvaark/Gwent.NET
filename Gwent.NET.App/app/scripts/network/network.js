(function () {
    'use strict';

    angular.module('app.network', [])
        .factory('userService', function ($rootScope, $http) {
            var userService = {};

            userService.setUser = function (newUser) {
                userService.user = newUser;
                if (newUser != null) {
                    $http.defaults.headers.common.Authorization = newUser.tokenType + ' ' + newUser.accessToken;
                } else {
                    $http.defaults.headers.common.Authorization = '';
                }
                $rootScope.$broadcast('userChanged');
            };

            return userService;
        })
        .factory('backendService', function ($http, $q, userService) {
            var backendUrl = 'http://localhost:9029/api';
            return {
                methods: {
                    login: function (username, password) {
                        var deferred = $q.defer();
                        $http.post(backendUrl + '/token',
                             "userName=" + encodeURIComponent(username) +
                             "&password=" + encodeURIComponent(password) +
                             "&grant_type=password")
                            .success(function (data, status, header) {

                                deferred.resolve({
                                    id: data.userId,
                                    name: data.userName,
                                    accessToken: data.access_token,
                                    tokenType: data.token_type
                                });
                            })
                            .error(function (msg) {
                                deferred.reject(msg);
                            });
                        return deferred.promise;
                    },
                    register: function (username, password) {
                        var deferred = $q.defer();
                        $http.post(backendUrl + '/user/register', { Username: username, Password: password })
                            .success(function (data) {
                                deferred.resolve({
                                    id: data.id,
                                    name: data.name,
                                    picture: data.picture,
                                    token: data.token
                                });
                            })
                            .error(function (msg) {
                                deferred.reject(msg);
                            });
                        return deferred.promise;
                    },
                    getCards: function () {
                        var deferred = $q.defer();
                        $http.get(backendUrl + '/card')
                            .success(function (data) {
                                deferred.resolve(data);
                            })
                            .error(function (msg) {
                                deferred.reject(msg);
                            });
                        return deferred.promise;
                    },
                    getDecks: function () {
                        if (userService.user == null) {
                            return $q.reject('No user logged in.');
                        }

                        var deferred = $q.defer();
                        $http.get(backendUrl + '/user/' + userService.user.id + '/deck')
                            .success(function (decks) {
                                deferred.resolve(decks);
                            })
                            .error(function (msg) {
                                deferred.reject(msg);
                            });
                        return deferred.promise;
                    },
                    createDeck: function (deck) {
                        if (userService.user == null) {
                            return $q.reject('No user logged in.');
                        }
                        var deferred = $q.defer();
                        $http.post(backendUrl + '/user/' + userService.user.id + '/deck', deck)
                            .success(function (responsedData) {
                                deferred.resolve(responsedData);
                            })
                            .error(function (msg) {
                                deferred.reject(msg);
                            });
                        return deferred.promise;
                    }
                }
            };
        })
        .factory('signalRService', function () {
            return {
                methods: {

                }
            };
        });
})();