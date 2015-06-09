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
                gameService.getGames().then(function (games) {
                    $log.info('Getting games successful');
                    data.games = games;
                    data.error = '';
                }, function (msg) {
                    $log.error('Unable to get games');
                    data.error = 'Unable to get games';
                });
            };

            methods.getActiveGame = function () {
                var deferred = $q.defer();

                gameService.getActiveGame().then(function (game) {
                    if (game == null) {
                        $log.info('No active game found');
                    } else {
                        $log.info('Active game found');
                    }
                    data.game = game;
                    deferred.resolve(game);
                }, function (msg) {
                    $log.error('Unable to get active game');
                    data.error = 'Unable to get active game';
                    deferred.reject();
                });
                return deferred.promise;
            };

            methods.createGame = function () {
                var deferred = $q.defer();
                gameService.createGame().then(function (game) {
                    $log.info('Game created');
                    data.game = game;
                    deferred.resolve(game);
                }, function (msg) {
                    $log.error('Unable to create game');
                    data.error = 'Unable to create game';
                    data.game = {};
                    deferred.reject();
                });
                return deferred.promise;
            };

            methods.joinGame = function (id) {
                gameService.joinGame(id).then(function (game) {
                    $log.info('Game joined');
                    data.game = game;
                }, function (msg) {
                    $log.error('Unable to join game');
                    data.error = 'Unable to join game';
                    data.game = {};
                });
            }

            methods.startGame = function () {
                methods.createGame().then(function () {
                    gameHubService.connect()
                        .then(function () {
                            $state.go('menu.game.lobby');
                        });
                });
            };

            methods.resumeGame = function () {
                gameHubService.connect()
                    .then(function () {
                        gameHubService.authenticate();
                        $state.go('menu.game.lobby');

                        /* Test command*/
                        //gameHubService.sendCommand({
                        //    type: "EndRedrawCard",
                        //    cardId: 1,
                        //    slot: "MeleeModifier",
                        //    playerId: 0
                        //});
                    });
            };

            methods.getActiveGame().then(function (game) {
            }, function () {
            });

            methods.getGames();
        });
})();