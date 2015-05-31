(function () {
    'use strict';

    angular.module('app.game', [])
        .config(function($stateProvider) {
            $stateProvider
                .state('game', {
                    url: '/game',
                    abstract: true,
                    template: '<div class="game">' +
                        '<div ui-view></div>' +
                        '</div>'
                })
                .state('game.lobby', {
                    url: '/:id',
                    template: '<div><div id=gameCanvas"></div></div>',
                    controller: 'GameController'
                });
        })
        .factory('cardService', function ($q, backendService) {
            var cardService = {
                cards : []
            };
            cardService.methods = {
                getCards: function() {
                    var deferred = $q.defer();
                    backendService.methods.getCards()
                        .then(function (cards) {
                            cardService.cards = cards;
                            deferred.resolve(cards);
                        }, function(msg) {
                            data.cards = [];
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                }
            }
            return cardService;
        })
        .filter('battleKing', function() {
            return function(cards) {
                var tempCards = [];
                angular.forEach(cards, function(card) {
                    if (card.IsBattleKing) {
                        tempCards.push(card);
                    }
                });
                return tempCards;
            }
        })
        .filter('notBattleKing', function() {
            return function(cards) {
                var tempCards = [];
                angular.forEach(cards, function(card) {
                    if (!card.IsBattleKing) {
                        tempCards.push(card);
                    }
                });
                return tempCards;
            }
        })
        .filter('faction', function() {
            return function (cards, faction) {
                var tempCards = [];
                if (faction === undefined || faction === '') {
                    return tempCards;
                }
                angular.forEach(cards, function (card) {

                    if (card.Faction === Number(faction)) {
                        tempCards.push(card);
                    }
                });
                return tempCards;
            }
        })
        .filter('factions', function() {
            return function (cards, factions) {
                var tempCards = [];
                if (factions === undefined || factions === []) {
                    return tempCards;
                }
                angular.forEach(factions, function(faction) {
                    angular.forEach(cards, function (card) {
                        if (card.Faction === Number(faction)) {
                            tempCards.push(card);
                        }
                    });
                });
                return tempCards;
            }
        })
        .factory('gwintFactionService', function() {
            var gwintFactionService = {};
            gwintFactionService.factions = [
                { id: 0, name: 'Neutral', validDeckFaction: false },
                { id: 1, name: 'Northern Kingdom', validDeckFaction: true },
                { id: 2, name: 'Nilfgaard', validDeckFaction: true },
                { id: 3, name: 'Scoia\'tael', validDeckFaction: true },
                { id: 4, name: 'No Man\'s Land', validDeckFaction: true },
            ];
            return gwintFactionService;
        })
        .filter('deckFaction', function() {
            return function (factions) {
                var tempFactions = [];
                angular.forEach(factions, function(faction) {
                    if (faction.validDeckFaction) {
                        tempFactions.push(faction);
                    }
                });
                return tempFactions;
            }
        })
        .factory('gwintTypeService', function () {
            var gwintTypeService = {};
            var types = gwintTypeService.types = new Map();
            types.set(0, 'None');
            types.set(1 << 0, 'GlobalEffect');
            types.set(1 << 1, 'FriendlyEffect');
            types.set(1 << 2, 'OffensiveEffect');
            types.set(1 << 3, 'RowModifier');
            types.set(1 << 4, 'Spell');
            types.set(1 << 5, 'Weather');
            types.set(1 << 6, 'Creature');
            types.set(1 << 7, 'Melee');
            types.set(1 << 8, 'Ranged');
            types.set(1 << 9, 'Siege');
            types.set(1 << 10, 'Hero');
            types.set(1 << 11, 'Spy');
            return gwintTypeService;
        })
        .factory('gwintEffectService', function () {
            var gwintEffectService = {};
            var effects = gwintEffectService.effects = new Map();
            effects.set(0, 'None');
            effects.set(1 << 0, 'CpClearWeather');
            effects.set(1 << 1, 'CpPickFrostCard');
            effects.set(1 << 2, 'CpPickFogCard');
            effects.set(1 << 3, 'CpPickRainCard');
            effects.set(1 << 4, 'CpPickWeatherCard');
            effects.set(1 << 5, 'CpMeleeHorn');
            effects.set(1 << 6, 'CpRangeHorn');
            effects.set(1 << 7, 'CpSiegeHorn');
            effects.set(1 << 8, 'CpMeleeScorch');
            effects.set(1 << 9, 'CpSiegeScorch');
            effects.set(1 << 10, 'CpResurectCard');
            effects.set(1 << 11, 'CpResurectFromEnemy');
            effects.set(1 << 12, 'CpView3EnemyCards');
            effects.set(1 << 13, 'CpCounterKingAblility');
            effects.set(1 << 14, 'Cp11ThCard');
            effects.set(1 << 15, 'CpBin2Pick1');
            effects.set(1 << 16, 'EffectMelee');
            effects.set(1 << 17, 'EffectRanged');
            effects.set(1 << 18, 'EffectSiege');
            effects.set(1 << 19, 'EffectScorch');
            effects.set(1 << 20, 'EffectHorn');
            effects.set(1 << 21, 'EffectImproveNeighbours');
            effects.set(1 << 22, 'EffectSummonClones');
            effects.set(1 << 23, 'EffectNurse');
            effects.set(1 << 24, 'EffectSameTypeMorale');
            effects.set(1 << 25, 'EffectDrawX2');
            effects.set(1 << 26, 'EffectUnsummonDummy');
            effects.set(1 << 27, 'EffectClearSky');
            return gwintEffectService;
        })

})();