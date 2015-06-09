(function () {
    'use strict';

    angular.module('app.network', [])
        .factory('userService', function ($rootScope, $log, $q, $window, $http) {
            var backendUrl = 'http://localhost:9029/api';
            var userService = {};
            var localStorage = $window['localStorage'];
            var user = null;

            var updateAuthorizationHeader = function () {
                // Token-based authentication
                if (user !== null) {
                    $http.defaults.headers.common.Authorization = user.tokenType + ' ' + user.accessToken;
                } else {
                    $http.defaults.headers.common.Authorization = '';
                }
            };

            var validateUser = function () {
                $http.get(backendUrl + '/user/me')
                    .success(function () {
                        $log.info('User validated successfully');
                    })
                    .error(function () {
                        $log.info('User could not be validated.');
                        userService.setUser(null);
                    });
            };

            var init = function () {
                user = JSON.parse(localStorage.getItem('user'));
                updateAuthorizationHeader();
                validateUser();
            };

            userService.setUser = function (newUser) {
                user = newUser;
                localStorage.setItem('user', JSON.stringify(user));
                updateAuthorizationHeader();
                $rootScope.$broadcast('userChanged', user);
            };

            userService.getUser = function () {
                return user;
            };

            userService.logout = function () {
                var deferred = $q.defer();
                $http.post(backendUrl + '/user/logout')
                    .success(function () {
                        userService.setUser(null);
                        deferred.resolve();
                    })
                    .error(function (msg) {
                        userService.setUser(null);
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };

            userService.login = function (username, password) {
                var deferred = $q.defer();
                $http.post(backendUrl + '/token',
                     "userName=" + encodeURIComponent(username) +
                     "&password=" + encodeURIComponent(password) +
                     "&grant_type=password")
                    .success(function (data) {
                        var user = {
                            id: data.userId,
                            name: data.userName,
                            accessToken: data.access_token,
                            tokenType: data.token_type
                        };
                        userService.setUser(user);
                        deferred.resolve(user);
                    })
                    .error(function (msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            }

            userService.register = function (username, password) {
                var deferred = $q.defer();
                $http.post(backendUrl + '/user/register', { Username: username, Password: password })
                    .success(function (data) {
                        var user = {
                            id: data.userId,
                            name: data.userName,
                            accessToken: data.access_token,
                            tokenType: data.token_type
                        };
                        userService.setUser(user);
                        deferred.resolve(user);
                    })
                    .error(function (msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };

            init();
            return userService;
        })
        .factory('gameService', function ($http, $q, userService) {
            var backendUrl = 'http://localhost:9029/api';
            return {
                getGames: function () {
                    if (userService.getUser() == null) {
                        return $q.reject('No user logged in.');
                    }
                    var deferred = $q.defer();
                    $http.get(backendUrl + '/game/browse')
                        .success(function (games) {
                            deferred.resolve(games);
                        })
                        .error(function (msg) {
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                },
                getActiveGame: function () {
                    if (userService.getUser() == null) {
                        return $q.reject('No user logged in.');
                    }
                    var deferred = $q.defer();
                    $http.get(backendUrl + '/game/active')
                        .success(function (game) {
                            deferred.resolve(game);
                        })
                        .error(function (msg, status) {
                            if (status === 404) {
                                deferred.resolve(null);
                            }
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                },
                createGame: function () {
                    if (userService.getUser() == null) {
                        return $q.reject('No user logged in.');
                    }
                    var deferred = $q.defer();
                    $http.post(backendUrl + '/game')
                        .success(function (game) {
                            deferred.resolve(game);
                        })
                        .error(function (msg) {
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                },
                joinGame: function (gameId) {
                    if (userService.getUser() == null) {
                        return $q.reject('No user logged in.');
                    }
                    var deferred = $q.defer();
                    $http.put(backendUrl + '/game/' + gameId + '/join')
                        .success(function (game) {
                            deferred.resolve(game);
                        })
                        .error(function (msg) {
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                }
            };
        })
        .factory('backendService', function ($http, $q, userService) {
            var backendUrl = 'http://localhost:9029/api';

            // Cookie-based authentication
            // $http.defaults.withCredentials = true;
            return {
                methods: {
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
                        if (userService.getUser() == null) {
                            return $q.reject('No user logged in.');
                        }
                        var deferred = $q.defer();
                        $http.get(backendUrl + '/deck')
                            .success(function (decks) {
                                deferred.resolve(decks);
                            })
                            .error(function (msg) {
                                deferred.reject(msg);
                            });
                        return deferred.promise;
                    },
                    createDeck: function (deck) {
                        if (userService.getUser() == null) {
                            return $q.reject('No user logged in.');
                        }
                        var deferred = $q.defer();
                        $http.post(backendUrl + '/deck', deck)
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
        .factory('gameHubService', function ($rootScope, $log, $q, userService) {
            var signalRUrl = 'http://localhost:9029/signalr';
            var gameHubService = {};
            var connection = null;
            var gameHubProxy = null;

            gameHubService.setAuthorizationQueryString = function () {
                if (connection) {
                    var user = userService.getUser();
                    if (user) {
                        connection.qs = { Bearer: userService.getUser().accessToken };
                    } else {
                        connection.qs = {};
                    }
                }
            };

            gameHubService.connect = function () {
                if (userService.getUser() == null) {
                    return $q.reject('No user logged in.');
                }
                if (connection !== null) {
                    console.log('already connected');
                    $q.when();
                }
                var deferred = $q.defer();
                $log.info('connecting');
                connection = $.hubConnection(signalRUrl, { useDefaultPath: false });
                gameHubService.setAuthorizationQueryString();
                gameHubProxy = connection.createHubProxy('gameHub');
                gameHubProxy.on('recieveServerCommand', gameHubService.recieveCommand);
                connection.start()
                    .done(function () {
                        console.log('connected');
                        deferred.resolve();
                    })
                    .fail(function () {
                        deferred.reject();
                    });
                return deferred.promise;
            };

            gameHubService.disconnect = function () {
                if (connection !== null) {
                    connection.stop();
                    connection = null;
                    gameHubProxy = null;
                }
            };

            gameHubService.recieveCommand = function (command) {
                var commandString = JSON.stringify(command);
                $log.info('server command recieved' + commandString);
                $rootScope.$broadcast('serverCommandRecieved', command);
            };

            gameHubService.sendCommand = function (command) {
                var commandString = JSON.stringify(command);
                $log.info('sending client command ' + commandString);
                gameHubProxy.invoke('RecieveClientCommand', command);
            };
            
            gameHubService.authenticate = function() {
                $log.info('authenticating client');
                gameHubProxy.invoke('Authenticate');
            }

            $rootScope.$on('userChanged', function () {
                gameHubService.setAuthorizationQueryString();
            });

            return gameHubService;
        });
})();