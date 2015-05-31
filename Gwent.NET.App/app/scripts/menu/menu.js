(function () {
    'use strict';

    angular.module('app.menu', [])
        .config(function ($stateProvider) {
            $stateProvider
                .state('menu', {
                    url: '',
                    templateUrl: 'templates/menu/menu.html',
                    controller: 'MenuController as ctrl'
                })
                .state('menu.login', {
                    url: '/login',
                    templateUrl: 'templates/menu/login.html'
                })
                .state('menu.register', {
                    url: '/register',
                    templateUrl: 'templates/menu/register.html'
                })
                .state('menu.main', {
                    url: '/main',
                    templateUrl: 'templates/menu/main.html'
                })
                .state('menu.main.deck', {
                    url: '/deck',
                    abstract: true,
                    templateUrl: 'templates/menu/deck.html',
                    controller: 'DeckController as ctrl'
                })
                .state('menu.main.deck.list', {
                    url: '',
                    templateUrl: 'templates/menu/deck-list.html'
                })
                .state('menu.main.deck.create', {
                    url: '/create',
                    templateUrl: 'templates/menu/deck-create.html'
                })
                .state('menu.main.lobby', {
                    url: '/lobby',
                    templateUrl: 'templates/menu/lobby.html'
                })
                .state('menu.main.browser', {
                    url: '/browser',
                    templateUrl: 'templates/menu/browser.html'
                });
        });
})();