(function () {
    'use strict';

    angular.module('app.game', [])
        .config(function ($stateProvider) {
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
                cards: []
            };
            cardService.methods = {
                getCards: function () {
                    var deferred = $q.defer();
                    backendService.methods.getCards()
                        .then(function (cards) {
                            cardService.cards = cards;
                            deferred.resolve(cards);
                        }, function (msg) {
                            cardService.cards = [];
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                }
            }
            return cardService;
        })
        .filter('battleKing', function () {
            return function (cards) {
                var tempCards = [];
                angular.forEach(cards, function (card) {
                    if (card.IsBattleKing) {
                        tempCards.push(card);
                    }
                });
                return tempCards;
            }
        })
        .filter('notBattleKing', function () {
            return function (cards) {
                var tempCards = [];
                angular.forEach(cards, function (card) {
                    if (!card.IsBattleKing) {
                        tempCards.push(card);
                    }
                });
                return tempCards;
            }
        })
        .filter('faction', function () {
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
        .filter('factions', function () {
            return function (cards, factions) {
                var tempCards = [];
                if (factions === undefined || factions === []) {
                    return tempCards;
                }
                angular.forEach(factions, function (faction) {
                    angular.forEach(cards, function (card) {
                        if (card.Faction === Number(faction)) {
                            tempCards.push(card);
                        }
                    });
                });
                return tempCards;
            }
        })
        .factory('gwintFactionService', function () {
            var gwintFactionService = {};
            gwintFactionService.factions = [
                { id: 0, name: 'Neutral', perk: '', validDeckFaction: false },
                { id: 1, name: 'Northern Kingdom', perk: 'Draw a card from your deck whenever you win a round.', validDeckFaction: true },
                { id: 2, name: 'Nilfgaard', perk: 'Win whenever there is a draw.', validDeckFaction: true },
                { id: 3, name: 'Scoia\'tael', perk: 'You devide who goes first at the start of battle.', validDeckFaction: true },
                { id: 4, name: 'No Man\'s Land', perk: 'One randomly-chosen Monster Unit Card stays on the battlefield after each round.', validDeckFaction: true },
            ];
            return gwintFactionService;
        })
        .filter('deckFaction', function () {
            return function (factions) {
                var tempFactions = [];
                angular.forEach(factions, function (faction) {
                    if (faction.validDeckFaction) {
                        tempFactions.push(faction);
                    }
                });
                return tempFactions;
            }
        })
        .factory('gwintTypeService', function () {
            var gwintTypeService = {}
            gwintTypeService.types = [
                { id: 0, name: 'None' },
                { id: 1 << 0, name: 'GlobalEffect' },
                { id: 1 << 1, name: 'FriendlyEffect' },
                { id: 1 << 2, name: 'OffensiveEffect' },
                { id: 1 << 3, name: 'RowModifier' },
                { id: 1 << 4, name: 'Spell' },
                { id: 1 << 5, name: 'Weather' },
                { id: 1 << 6, name: 'Creature' },
                { id: 1 << 7, name: 'Melee' },
                { id: 1 << 8, name: 'Ranged' },
                { id: 1 << 9, name: 'Siege' },
                { id: 1 << 10, name: 'Hero' },
                { id: 1 << 11, name: 'Spy' }
            ];
            gwintTypeService.methods = {
                getTypes: function (types) {
                    var tempTypes = [];
                    angular.forEach(gwintTypeService.types, function (type) {
                        if (types & type.id) {
                            tempTypes.push(type);
                        }
                    });
                    return tempTypes;
                },
                hasType: function (types, typeName) {
                    var type;
                    for (var i = 0; i < gwintTypeService.types.length; i++) {
                        type = gwintTypeService.types[i];
                        if (type.name === typeName && (types & type.id)) {
                            return true;
                        }
                    }
                    return false;
                },
                hasAnyType: function (types, typeNames) {
                    var type;
                    for (var i = 0; i < gwintTypeService.types.length; i++) {
                        type = gwintTypeService.types[i];
                        for (var j = 0; j < typeNames.length; j++) {
                            if (type.name === typeNames[j] && (types & type.id)) {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            };

            return gwintTypeService;
        })
        .filter('gwintType', function () {
            return function (cards, allowedTypes) {
                var tempCards = [];

                if (allowedTypes === undefined || allowedTypes === []) {
                    return tempCards;
                }

                angular.forEach(cards, function (card) {
                    for (var i = 0; i < allowedTypes.length; i++) {
                        if (card.Type & allowedTypes[i]) {
                            tempCards.push(card);
                            return;
                        }
                    }
                });

                return tempCards;
            }
        })
        .factory('gwintEffectService', function () {
            var gwintEffectService = {
                effects: [
                    { id: 0, name: 'None' },
                    { id: 1 << 0, name: 'CpClearWeather' },
                    { id: 1 << 1, name: 'CpPickFrostCard' },
                    { id: 1 << 2, name: 'CpPickFogCard' },
                    { id: 1 << 3, name: 'CpPickRainCard' },
                    { id: 1 << 4, name: 'CpPickWeatherCard' },
                    { id: 1 << 5, name: 'CpMeleeHorn' },
                    { id: 1 << 6, name: 'CpRangeHorn' },
                    { id: 1 << 7, name: 'CpSiegeHorn' },
                    { id: 1 << 8, name: 'CpMeleeScorch' },
                    { id: 1 << 9, name: 'CpSiegeScorch' },
                    { id: 1 << 10, name: 'CpResurectCard' },
                    { id: 1 << 11, name: 'CpResurectFromEnemy' },
                    { id: 1 << 12, name: 'CpView3EnemyCards' },
                    { id: 1 << 13, name: 'CpCounterKingAblility' },
                    { id: 1 << 14, name: 'Cp11ThCard' },
                    { id: 1 << 15, name: 'CpBin2Pick1' },
                    { id: 1 << 16, name: 'EffectMelee' },
                    { id: 1 << 17, name: 'EffectRanged' },
                    { id: 1 << 18, name: 'EffectSiege' },
                    { id: 1 << 19, name: 'EffectScorch' },
                    { id: 1 << 20, name: 'EffectHorn' },
                    { id: 1 << 21, name: 'EffectImproveNeighbours' },
                    { id: 1 << 22, name: 'EffectSummonClones' },
                    { id: 1 << 23, name: 'EffectNurse' },
                    { id: 1 << 24, name: 'EffectSameTypeMorale' },
                    { id: 1 << 25, name: 'EffectDrawX2' },
                    { id: 1 << 26, name: 'EffectUnsummonDummy' },
                    { id: 1 << 27, name: 'EffectClearSky' },
                ]
            };
            gwintEffectService.methods = {
                getEffects: function (effects) {
                    var tempEffects = [];
                    angular.forEach(gwintEffectService.effects, function (effect) {
                        if (effects & effect.id) {
                            tempEffects.push(effect);
                        }
                    });
                    return tempEffects;
                }
            }

            return gwintEffectService;
        })
        .filter('gwintEffect', function () {
            return function (cards, allowedEffects) {
                var tempCards = [];

                if (allowedEffects === undefined || allowedEffects === []) {
                    return tempCards;
                }

                angular.forEach(cards, function (card) {
                    for (var i = 0; i < allowedEffects.length; i++) {
                        if (card.Effect & allowedEffects[i]) {
                            tempCards.push(card);
                            return;
                        }
                    }
                });

                return tempCards;
            }
        })
        .directive('gwCard', function () {
            return {
                require: '^ngModel',
                scope: {
                    card: '=ngModel'
                },
                templateUrl: 'templates/game/gw-card.html'
            };
        })
        .directive('gwCardField', function () {
            return {
                require: '^ngModel',
                scope: {
                    card: '=ngModel'
                },
                templateUrl: 'templates/game/gw-card-field.html'
            };
        });
})();