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
                { id: 4, name: 'Scoia\'tael', perk: 'You devide who goes first at the start of battle.', validDeckFaction: true }
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
                { id: 1, name: 'Backstab' },
                { id: 2, name: 'MoraleBoost' },
                { id: 3, name: 'Ambush' },
                { id: 4, name: 'ToughSkin' },
                { id: 5, name: 'Bin2' },
                { id: 6, name: 'Bin3' },
                { id: 7, name: 'MeleeScorch' },
                { id: 8, name: 'EleventhCard' },
                { id: 9, name: 'ClearWeather' },
                { id: 10, name: 'PickWeather' },
                { id: 11, name: 'PickRain' },
                { id: 12, name: 'PickFog' },
                { id: 13, name: 'PickFrost' },
                { id: 14, name: 'View3Enemy' },
                { id: 15, name: 'Resurrect' },
                { id: 16, name: 'ResurrectEnemy' },
                { id: 17, name: 'Bin2Pick1' },
                { id: 18, name: 'MeleeHorn' },
                { id: 19, name: 'RangeHorn' },
                { id: 20, name: 'SiegeHorn' },
                { id: 21, name: 'SiegScorch' },
                { id: 22, name: 'CounerKing' },
                { id: 23, name: 'Melee' },
                { id: 24, name: 'Ranged' },
                { id: 25, name: 'Siege' },
                { id: 26, name: 'UnsummonDummy' },
                { id: 27, name: 'Horn' },
                { id: 28, name: 'Draw' },
                { id: 29, name: 'Scorch' },
                { id: 30, name: 'ClearSky' },
                { id: 31, name: 'SummonClones' },
                { id: 32, name: 'ImproveNeighbours' },
                { id: 33, name: 'Nurse' },
                { id: 34, name: 'Draw2' },
                { id: 35, name: 'SameTypeMorale' }
            ];
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