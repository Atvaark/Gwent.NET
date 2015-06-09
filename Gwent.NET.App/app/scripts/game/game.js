(function () {
    'use strict';

    angular.module('app.game', [])
        .factory('cardService', function ($q, backendService) {
            var cardService = {
                cardsLoaded: false,
                cards: []
            };
            cardService.methods = {
                getCards: function () {
                    if (cardService.cardsLoaded) {
                        return $q.when(cardService.cards);
                    }

                    var deferred = $q.defer();
                    backendService.methods.getCards()
                        .then(function (cards) {
                            cardService.cards = cards;
                            cardService.cardsLoaded = true;
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
                { id: 1, name: 'No Man\'s Land', perk: 'One randomly-chosen Monster Unit Card stays on the battlefield after each round.', validDeckFaction: true },
                { id: 2, name: 'Nilfgaard', perk: 'Win whenever there is a draw.', validDeckFaction: true },
                { id: 3, name: 'Northern Kingdom', perk: 'Draw a card from your deck whenever you win a round.', validDeckFaction: true },
                { id: 4, name: 'Scoia\'tael', perk: 'You devide who goes first at the start of battle.', validDeckFaction: true },
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
                { id: 1 << 0, name: 'Backstab' },
                { id: 1 << 1, name: 'MoraleBoost' },
                { id: 1 << 2, name: 'Ambush' },
                { id: 1 << 3, name: 'ToughSkin' },
                { id: 1 << 4, name: 'Bin2' },
                { id: 1 << 5, name: 'Bin3' },
                { id: 1 << 6, name: 'MeleeScorch' },
                { id: 1 << 7, name: 'EleventhCard' },
                { id: 1 << 8, name: 'ClearWeather' },
                { id: 1 << 9, name: 'PickWeather' },
                { id: 1 << 10, name: 'PickRain' },
                { id: 1 << 11, name: 'PickFog' },
                { id: 1 << 12, name: 'PickFrost' },
                { id: 1 << 13, name: 'View3Enemy' },
                { id: 1 << 14, name: 'Resurrect' },
                { id: 1 << 15, name: 'ResurrectEnemy' },
                { id: 1 << 16, name: 'Bin2Pick1' },
                { id: 1 << 17, name: 'MeleeHorn' },
                { id: 1 << 18, name: 'RangeHorn' },
                { id: 1 << 19, name: 'SiegeHorn' },
                { id: 1 << 20, name: 'SiegScorch' },
                { id: 1 << 21, name: 'CounerKing' },
                { id: 1 << 22, name: 'Melee' },
                { id: 1 << 23, name: 'Ranged' },
                { id: 1 << 24, name: 'Siege' },
                { id: 1 << 25, name: 'UnsummonDummy' },
                { id: 1 << 26, name: 'Horn' },
                { id: 1 << 27, name: 'Draw' },
                { id: 1 << 28, name: 'Scorch' },
                { id: 1 << 29, name: 'ClearSky' },
                { id: 1 << 30, name: 'SummonClones' },
                { id: 1 << 31, name: 'ImproveNeighbours' },
                { id: 1 << 32, name: 'Nurse' },
                { id: 1 << 33, name: 'Draw2' },
                { id: 1 << 34, name: 'SameTypeMorale' }
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
                    { id: 1 << 0, name: 'Melee' },
                    { id: 1 << 1, name: 'Ranged' },
                    { id: 1 << 0 | 1 << 1, name: 'RangedMelee' },
                    { id: 1 << 2, name: 'Siege' },
                    { id: 1 << 0 | 1 << 1 | 1 << 2, name: 'SiegeRangedMelee' },
                    { id: 1 << 3, name: 'Creature' },
                    { id: 1 << 4, name: 'Weather' },
                    { id: 1 << 5, name: 'Spell' },
                    { id: 1 << 6, name: 'RowModifier' },
                    { id: 1 << 7, name: 'Hero' },
                    { id: 1 << 8, name: 'Spy' },
                    { id: 1 << 9, name: 'FriendlyEffect' },
                    { id: 1 << 10, name: 'OffensiveEffect' },
                    { id: 1 << 11, name: 'GlobalEffect' }
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
        })
        .directive('gwBoard', function () {
            var controller = function () {

            };

            return {
                require: '^ngModel',
                scope: {
                    game: '=ngModel'
                },
                controller: controller,
                templateUrl: 'templates/game/gw-board.html'
            };
        });
})();