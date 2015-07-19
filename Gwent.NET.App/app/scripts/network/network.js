(function () {
    'use strict';

    angular.module('app.network', [])
        .factory('backendUrlService', function ($location) {
            var backendUrlService = {};
            var host = $location.host();
            var port = '13471';
            var origin = 'http://' + host + ':' + port;
            backendUrlService.backendUrl = origin + '/api';
            backendUrlService.signalRUrl = origin + '/signalr';;
            return backendUrlService;
        })
        .factory('userService', function ($rootScope, $log, $q, $window, $http, backendUrlService) {
            var backendUrl = backendUrlService.backendUrl;
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
                        $log.info('user validated successfully');
                    })
                    .error(function () {
                        $log.info('user could not be validated.');
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
                $http.post(backendUrl + '/user/register', { username: username, password: password })
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
        .factory('gameService', function ($http, $q, userService, backendUrlService) {
            var backendUrl = backendUrlService.backendUrl;
            return {
                getGames: function () {
                    if (userService.getUser() == null) {
                        return $q.reject('no user logged in.');
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
                        return $q.reject('no user logged in.');
                    }
                    var deferred = $q.defer();
                    $http.get(backendUrl + '/game/active')
                        .success(function (game, status) {
                            if (status === 204) {
                                deferred.resolve(null);
                            }
                            deferred.resolve(game);
                        })
                        .error(function (msg) {
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                },
                createGame: function () {
                    if (userService.getUser() == null) {
                        return $q.reject('no user logged in.');
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
                        return $q.reject('no user logged in.');
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
        .factory('backendService', function ($http, $q, userService, backendUrlService) {
            var backendUrl = backendUrlService.backendUrl;

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
        .factory('gameHubService', function ($rootScope, $log, $q, userService, backendUrlService) {
            var signalRUrl = backendUrlService.signalRUrl;
            var gameHubService = {};
            var connected = false;
            var connection = null;
            var gameHubProxy = null;

            var getAuthorizationQueryString = function() {
                var user = userService.getUser();
                if (user) {
                    return { Bearer: userService.getUser().accessToken };
                } else {
                    return {};
                }
            };

            var setAuthorizationQueryString = function () {
                if (connection) {
                    connection.qs = getAuthorizationQueryString();
                }
            };

            gameHubService.connect = function () {
                if (userService.getUser() == null) {
                    return $q.reject('no user logged in.');
                }

                if (connected) {
                    $log.info('already connected');
                    return $q.when();
                }
                var deferred = $q.defer();
                $log.info('creating new connection');
                connection = $.hubConnection(signalRUrl, {
                    useDefaultPath: false,
                    qs: getAuthorizationQueryString()
                });
                gameHubProxy = connection.createHubProxy('gameHub');
                gameHubProxy.on('recieveServerEvent', gameHubService.recieveServerEvent);
                $log.info('starting connection');
                connection.start()
                    .done(function () {
                        $log.info('connected');
                        connected = true;
                        deferred.resolve();
                    })
                    .fail(function () {
                        $log.error('connection failed');
                        gameHubService = null;
                        connection = null;
                        deferred.reject();
                    });
                return deferred.promise;
            };

            gameHubService.disconnect = function () {
                if (connection !== null) {
                    connection.stop();
                    connected = false;
                    connection = null;
                    gameHubProxy = null;
                }
            };

            gameHubService.recieveServerEvent = function (event) {
                var eventString = JSON.stringify(event);
                $log.info('server event recieved: ' + eventString);
                $rootScope.$broadcast('serverEventRecieved', event);
            };
            
            gameHubService.sendCommand = function (command) {
                if (!gameHubProxy) {
                    return $q.reject('not connected to the game hub.');
                }

                var deferred = $q.defer();
                var commandString = JSON.stringify(command);
                $log.info('sending client command ' + commandString);
                gameHubProxy.invoke('RecieveClientCommand', command)
                    .done(function (result) {
                        if (result.error) {
                            deferred.reject(result.error);
                        } else {
                            deferred.resolve(result.data);
                        }
                    })
                    .fail(function () {
                        deferred.reject();
                    });

                return deferred.promise;
            };

            gameHubService.browseGames = function () {
                if (!gameHubProxy) {
                    $log.error('browse game failed');
                    return $q.reject('not connected to the game hub.');
                }

                var deferred = $q.defer();
                $log.info('browsing games');
                gameHubProxy.invoke('BrowseGames')
                    .done(function (result) {
                        if (result.error) {
                            deferred.reject(result.error);
                        } else {
                            deferred.resolve(result.data);
                        }
                    })
                    .fail(function () {
                        deferred.reject();
                    });

                return deferred.promise;
            }

            gameHubService.getActiveGame = function () {
                if (!gameHubProxy) {
                    return $q.reject('not connected to the game hub.');
                }

                var deferred = $q.defer();
                gameHubProxy.invoke('GetActiveGame')
                    .done(function (result) {
                        if (result.error) {
                            deferred.reject(result.error);
                        } else {
                            deferred.resolve(result.data);
                        }
                    })
                    .fail(function () {
                        deferred.reject();
                    });

                return deferred.promise;
            }

            gameHubService.createGame = function () {
                if (!gameHubProxy) {
                    return $q.reject('not connected to the game hub.');
                }

                var deferred = $q.defer();
                $log.info('creating game');
                gameHubProxy.invoke('CreateGame')
                    .done(function (result) {
                        if (result.error) {
                            deferred.reject(result.error);
                        } else {
                            deferred.resolve(result.data);
                        }
                    })
                    .fail(function () {
                        deferred.reject();
                    });

                return deferred.promise;
            }

            gameHubService.joinGame = function (gameId) {
                if (!gameHubProxy) {
                    return $q.reject('not connected to the game hub.');
                }

                var deferred = $q.defer();
                $log.info('joining game');
                gameHubProxy.invoke('JoinGame', gameId)
                    .done(function (result) {
                        if (result.error) {
                            deferred.reject(result.error);
                        } else {
                            deferred.resolve(result.data);
                        }
                    })
                    .fail(function () {
                        deferred.reject();
                    });

                return deferred.promise;
            }

            $rootScope.$on('userChanged', function () {
                setAuthorizationQueryString();
            });

            return gameHubService;
        });
})();