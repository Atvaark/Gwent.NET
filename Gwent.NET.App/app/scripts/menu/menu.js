(function () {
    'use strict';

    angular.module('app.menu', [])
        .config(function ($stateProvider, $locationProvider) {
            //$locationProvider.html5Mode(true);
            $stateProvider
                .state('menu', {
                    url: '',
                    abstract: true,
                    templateUrl: 'templates/menu/menu.html',
                    controller: 'MenuController as ctrl'
                })
                .state('menu.home', {
                    url: '',
                    templateUrl: 'templates/menu/home.html',
                })
                .state('menu.login', {
                    url: '/login',
                    templateUrl: 'templates/menu/login.html'
                })
                .state('menu.register', {
                    url: '/register',
                    templateUrl: 'templates/menu/register.html'
                })
                .state('menu.deck', {
                    url: '/deck',
                    abstract: true,
                    templateUrl: 'templates/menu/deck.html',
                    controller: 'DeckController as ctrl'
                })
                .state('menu.deck.list', {
                    url: '',
                    templateUrl: 'templates/menu/deck-list.html'
                })
                .state('menu.deck.create', {
                    url: '/create',
                    templateUrl: 'templates/menu/deck-create.html'
                })
                .state('menu.game', {
                    url: '/game',
                    templateUrl: 'templates/menu/game.html',
                    controller: 'GameController as ctrl'
                })
                .state('menu.game.browser', {
                    url: '/browser',
                    templateUrl: 'templates/menu/game-browser.html'
                })
                .state('menu.game.lobby', {
                    url: '/lobby',
                    templateUrl: 'templates/menu/game-lobby.html'
                });
        });
})();