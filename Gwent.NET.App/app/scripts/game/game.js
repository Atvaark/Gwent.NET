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
                    if (card.isBattleKing) {
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
                    if (!card.isBattleKing) {
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

                    if (card.faction === Number(faction)) {
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
                        if (card.faction === Number(faction)) {
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
            gwintTypeService.types = {
                None: 0,
                Melee: 1 << 0,
                Ranged: 1 << 1,
                RangedMelee: 1 << 0 | 1 << 1,
                Siege: 1 << 2,
                SiegeRangedMelee: 1 << 0 | 1 << 1 | 1 << 2,
                Creature: 1 << 3,
                Weather: 1 << 4,
                Spell: 1 << 5,
                RowModifier: 1 << 6,
                Hero: 1 << 7,
                Spy: 1 << 8,
                FriendlyEffect: 1 << 9,
                OffensiveEffect: 1 << 10,
                GlobalEffect: 1 << 11
            };

            gwintTypeService.hasType = function (types, typeName) {
                return (types & gwintTypeService.types[typeName]) > 0;
            };

            gwintTypeService.hasAnyType = function (type, typeNames) {
                var typeMask = 0;

                if (typeNames === undefined || typeNames === null || typeNames === []) {
                    return false;
                }

                angular.forEach(typeNames, function (typeName) {
                    typeMask = typeMask | gwintTypeService.types[typeName];
                });

                return (type & typeMask) > 0;
            };

            gwintTypeService.hasAllTypes = function (type, typeNames) {
                var typeMask = 0;

                if (typeNames === undefined || typeNames === null || typeNames === []) {
                    return false;
                }

                angular.forEach(typeNames, function (typeName) {
                    typeMask = typeMask | gwintTypeService.types[typeName];
                });

                return (type & typeMask) === typeMask;
            };

            return gwintTypeService;
        })
        .filter('gwintType', function (gwintTypeService) {
            return function (cards, allowedTypes) {
                var tempCards = [];
                var allowedTypesMask = 0;

                if (allowedTypes === undefined || allowedTypes === null || allowedTypes === []) {
                    return tempCards;
                }

                angular.forEach(allowedTypes, function (allowedType) {
                    allowedTypesMask = allowedTypesMask | gwintTypeService.types[allowedType];
                });

                angular.forEach(cards, function (card) {
                    if (card.type & allowedTypesMask) {
                        tempCards.push(card);
                    }
                });

                return tempCards;
            }
        })
        .factory('gwintEffectService', function () {
            var gwintEffectService = {
                effects: {
                    None: 0,
                    Backstab: 1,
                    MoraleBoost: 2,
                    Ambush: 3,
                    ToughSkin: 4,
                    Bin2: 5,
                    Bin3: 6,
                    MeleeScorch: 7,
                    EleventhCard: 8,
                    ClearWeather: 9,
                    PickWeather: 10,
                    PickRain: 11,
                    PickFog: 12,
                    PickFrost: 13,
                    View3Enemy: 14,
                    Resurrect: 15,
                    ResurrectEnemy: 16,
                    Bin2Pick1: 17,
                    MeleeHorn: 18,
                    RangeHorn: 19,
                    SiegeHorn: 20,
                    SiegScorch: 21,
                    CounerKing: 22,
                    Melee: 23,
                    Ranged: 24,
                    Siege: 25,
                    UnsummonDummy: 26,
                    Horn: 27,
                    Draw: 28,
                    Scorch: 29,
                    ClearSky: 30,
                    SummonClones: 31,
                    ImproveNeighbours: 32,
                    Nurse: 33,
                    Draw2: 34,
                    SameTypeMorale: 35
                }
            };

            gwintEffectService.hasEffect = function (effect, effectName) {
                return effect === gwintEffectService.effects[effectName];
            };

            gwintEffectService.hasNotEffect = function (effect, effectName) {
                return effect !== gwintEffectService.effects[effectName];
            };

            gwintEffectService.hasAnyEffect = function (effect, effectNames) {
                if (effectNames === undefined || effectNames === null || effectNames === []) {
                    return false;
                }

                for (var i = 0; i < effectNames.length; i++) {
                    if (effect === gwintEffectService.effects[effectNames[i]]) {
                        return true;
                    }
                }
                return false;
            };

            return gwintEffectService;
        })
        .filter('gwintEffect', function (gwintEffectService) {
            return function (cards, allowedEffects) {
                var tempCards = [];

                if (allowedEffects === undefined || allowedEffects === null || allowedEffects === []) {
                    return tempCards;
                }

                angular.forEach(cards, function (card) {
                    if (gwintEffectService.hasAnyEffect(card.effect, allowedEffects)) {
                        tempCards.push(card);
                    }
                });

                return tempCards;
            }
        })
        .factory('gwintSideService', function (gwintTypeService) {
            var gwintSideService = {};
            gwintSideService.sides = {
                self: {
                    canPlayCard: function (card) {
                        return !gwintTypeService.hasAnyType(card.type, ['Spy', 'OffensiveEffect']);
                    }
                },
                opponent: {
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAnyType(card.type, ['Spy', 'OffensiveEffect', 'GlobalEffect']);
                    }
                }
            };

            gwintSideService.canPlayCard = function (card, sideName) {
                for (var side in gwintSideService.sides) {
                    if (!gwintSideService.sides.hasOwnProperty(side)) {
                        continue;
                    }

                    if (side !== sideName) {
                        continue;
                    }

                    return gwintSideService.sides[side].canPlayCard(card);
                }

                return false;
            }

            return gwintSideService;
        })
        .factory('gwintSlotService', function (gwintTypeService) {
            var gwintSlotService = {};

            gwintSlotService.slots = {
                None: {
                    id: 0,
                    canPlayCard: function () {
                        return false;
                    }
                },
                Melee: {
                    id: 1,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAllTypes(card.type, ['Melee', 'Creature']);
                    }
                },
                Ranged: {
                    id: 2,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAllTypes(card.type, ['Ranged', 'Creature']);
                    }
                },
                Siege: {
                    id: 3,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAllTypes(card.type, ['Siege', 'Creature']);
                    }
                },
                MeleeModifier: {
                    id: 4,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAllTypes(card.type, ['Melee', 'RowModifier']);
                    }
                },
                RangedModifier: {
                    id: 5,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAllTypes(card.type, ['Melee', 'RowModifier']);
                    }
                },
                SiegeModifier: {
                    id: 6,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasAllTypes(card.type, ['Melee', 'RowModifier']);
                    }
                },
                Weather: {
                    id: 7,
                    canPlayCard: function (card) {
                        return gwintTypeService.hasType(card.type, 'Weather');
                    }
                }
            };

            gwintSlotService.canPlayCard = function (card, slotId) {
                for (var slotName in gwintSlotService.slots) {
                    if (!gwintSlotService.slots.hasOwnProperty(slotName)) {
                        continue;
                    }

                    var slot = gwintSlotService.slots[slotName];
                    if (slot.id !== slotId) {
                        continue;
                    }

                    return slot.canPlayCard(card);
                }

                return false;
            };

            return gwintSlotService;
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