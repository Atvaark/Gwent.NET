(function () {
    'use strict';

    angular.module('app.menu')
        .controller('GameController', function ($scope, $state, $stateParams, $log, $q, gameService, gameHubService) {
            var data = $scope.data =
            {
                game: null,
                games: []
            };
            var methods = $scope.methods = {};

            methods.getGames = function () {
                var deferred = $q.defer();
                gameHubService.browseGames().then(function(games) {
                    $log.info('getting games successful');
                    data.games = games;
                    data.error = '';
                    deferred.resolve();

                }, function () {
                    $log.error('unable to get games');
                    data.error = 'unable to get games';
                    deferred.reject();
                    });

                return deferred.promise;
            };

            methods.getActiveGame = function () {
                var deferred = $q.defer();

                gameHubService.getActiveGame().then(function (game) {
                    if (game == null) {
                        $log.info('no active game found');
                    } else {
                        $log.info('active game found');
                    }
                    data.game = game;
                    deferred.resolve(game);
                }, function (error) {
                    $log.error(error);
                    data.error = error;
                    deferred.reject(error);
                });
                return deferred.promise;
            };

            methods.createGame = function () {
                var deferred = $q.defer();
                gameHubService.createGame().then(function (game) {
                    $log.info('game created');
                    data.game = game;
                    deferred.resolve(game);
                }, function (error) {
                    $log.error('unable to create game');
                    data.error = 'unable to create game';
                    data.game = {};
                    deferred.reject();
                });
                return deferred.promise;
            };

            methods.joinGame = function (id) {
                gameHubService.joinGame(id).then(function (game) {
                    $log.info('game joined');
                    data.game = game;
                    $state.go('menu.game.lobby');
                }, function (error) {
                    $log.error('unable to join game');
                    data.error = 'unable to join game';
                    data.game = {};
                });
            }

            methods.browseGames = function() {
                methods.getGames().then(function() {
                    $state.go('menu.game.browser');
                }, function() {
                    $state.go('menu.game.browser');
                });
            };

            methods.startGame = function () {
                gameHubService.connect()
                    .then(function () {
                        methods.createGame().then(function () {
                            $log.info('game created');
                            $state.go('menu.game.lobby');
                        }, function(error) {
                            $log.error('unable to create game: ' + error);
                        });
                    });

            };

            methods.resumeGame = function () {
                gameHubService.connect()
                    .then(function () {
                        gameHubService.authenticate().then(function () {
                            $state.go('menu.game.lobby');
                        });
                    });
            };

            methods.sendStartCommand = function () {
                gameHubService.sendCommand({
                    type: "StartGame"
                }).then(function () {
                    $log.info('game started');
                }, function (error) {
                    $log.error('unable to start game: ' + error);
                });
            };

            methods.sendPassCommand = function () {
                gameHubService.sendCommand({
                    type: "Pass"
                }).then(function () {
                    $log.info('round passed');
                }, function (error) {
                    $log.error('unable to pass round: ' + error);
                });
            };

            methods.sendForfeitGameCommand = function () {
                gameHubService.sendCommand({
                    type: "ForfeitGame"
                }).then(function () {
                    $log.info('game forfeited');
                    data.game = null;
                    $state.go('menu.game');
                }, function (error) {
                    $log.error('unable to forfeit game: ' + error);
                });
            };

            methods.sendPickStartingPlayerCommand = function (userId) {
                gameHubService.sendCommand({
                    type: "PickStartingPlayer",
                    startPlayerId: userId
                }).then(function () {
                    $log.info('starting player picked');
                }, function (error) {
                    $log.error('unable to pick starting player: ' + error);
                });
            };

            methods.sendRedrawCardCommand = function (cardId) {
                gameHubService.sendCommand({
                    type: "RedrawCard",
                    cardId: cardId
                }).then(function () {
                    $log.info('card redrawn');
                }, function (error) {
                    $log.error('unable to redraw card: ' + error);
                });
            };

            methods.sendEndRedrawCardCommand = function () {
                gameHubService.sendCommand({
                    type: "EndRedrawCard"
                }).then(function () {
                    $log.info('ended redrawing cards');
                }, function (error) {
                    $log.error('unable to end redrawing cards: ' + error);
                });
            };

            methods.sendPlayCardCommand = function (cardId, slot) {
                gameHubService.sendCommand({
                    type: "PlayCard",
                    cardId: cardId,
                    //resurrectCardId: 0,
                    gwintSlot: slot
                }).then(function () {
                    $log.info('played card');
                }, function (error) {
                    $log.error('unable to play card: ' + error);
                });
            };

            methods.sendUseBattleKingCardCommand = function () {
                gameHubService.sendCommand({
                    type: "UseBattleKingCard"
                }).then(function () {
                    $log.info('used battle king card');
                }, function (error) {
                    $log.error('unable to use battle king card: ' + error);
                });
            };


            // Initializing game page
            gameHubService.connect().then(function () {
                gameHubService.authenticate().then(function () {
                    methods.getActiveGame().then(function (game) {
                        $log.info('resuming game');
                        methods.resumeGame();;
                    });
                }, function () {
                    $log.error('could not authenticate');
                });
            }, function () {
                $log.error('could not connect');
            });
        });
})();