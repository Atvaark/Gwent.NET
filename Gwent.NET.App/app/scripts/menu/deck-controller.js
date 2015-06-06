(function () {
    'use strict';

    angular.module('app.menu')
        .controller('DeckController', function ($scope, $state, $log, cardService, backendService, gwintFactionService, gwintTypeService) {
            var data = $scope.data = {
                initialized: false,
                maxSpecialCards: 10,
                cards: [],
                decks: [],
                create: {
                    cards: []
                }
            };
            var methods = $scope.methods = $scope.methods || {};
            methods.getCards = function () {
                cardService.methods.getCards().then(function (cards) {
                    $log.info('Cards updated.');
                    data.cards = cards;
                    data.error = '';
                }, function (msg) {
                    $log.error('Unable to retrieve cards.');
                    data.cards = [];
                    data.error = 'Unable to retrieve cards.';
                });
            };

            methods.getDecks = function () {
                backendService.methods.getDecks().then(function (decks) {
                    $log.info('Decks updated.');
                    data.decks = decks;
                    data.error = '';
                }, function (msg) {
                    $log.error('Unable to retrieve decks.');
                    data.decks = [];
                    data.error = 'Unable to retrieve decks.';
                });
            };

            methods.showDecks = function () {
                methods.getDecks();
                $state.go('menu.deck.list');
            };

            methods.newDeck = function () {
                data.create = {
                    cards: []
                };
                $state.go('menu.deck.create');
            };

            methods.addDeckCard = function (card) {
                var deckCard = methods.findDeckCard(card.Index);
                if (deckCard) {
                    deckCard.Count = deckCard.Count + 1;
                    return;
                }
                deckCard = {
                    Index: card.Index,
                    Title: card.Title,
                    Description: card.Description,
                    Power: card.Power,
                    Picture: card.Picture,
                    Faction: card.Faction,
                    Type: card.Type,
                    Effect: card.Effect,
                    SummonFlags: card.SummonFlags,
                    IsBattleKing: card.IsBattleKing,
                    Count: 1
                };
                data.create.cards.push(deckCard);
            };

            methods.removeDeckCard = function (cardIndex) {
                data.create.cards.splice(cardIndex, 1);
            };

            methods.findDeckCard = function (cardIndex) {
                var foundCard = null;
                angular.forEach(data.create.cards, function (deckCard) {
                    if (deckCard.Index === cardIndex) {
                        foundCard = deckCard;
                        return;
                    }
                });
                return foundCard;
            };

            methods.createDeck = function () {
                var deckDto = {
                    Cards: [],
                    Faction: data.create.faction,
                    BattleKingCard: data.create.battleKing
                };
                angular.forEach(data.create.cards, function (card) {
                    for (var i = 0; i < card.Count; i++) {
                        deckDto.Cards.push(card.Index);
                    }
                });
                backendService.methods.createDeck(deckDto).then(function (response) {
                    $log.info('Deck created.');
                    data.error = '';
                    methods.showDecks();
                }, function (msg) {
                    $log.error('Unable to create deck.');
                    data.error = 'Unable to create deck.';
                });
            };

            methods.getTotalCardsInDeck = function () {
                var count = 0;
                angular.forEach(data.create.cards, function(card) {
                    count += card.Count;
                });
                return count;
            };

            methods.getUnitCardsCount = function () {
                var unitCardCount = 0;
                angular.forEach(data.create.cards, function (card) {
                    if (gwintTypeService.methods.hasType(card.Type, 'Creature')) {
                        unitCardCount += card.Count;
                    }
                });
                return unitCardCount;
            };

            methods.getSpecialCardsCount = function () {
                var specialCardCount = 0;
                angular.forEach(data.create.cards, function (card) {
                    if (gwintTypeService.methods.hasAnyType(card.Type, ['Spell', 'RowModifier', 'GlobalEffect', 'Weather'])) {
                        specialCardCount += card.Count;
                    }
                });
                return specialCardCount;
            };
            
            methods.getTotalUnitCardStrength = function () {
                var powerSum = 0;
                angular.forEach(data.create.cards, function (card) {
                    powerSum += card.Power * card.Count;
                });
                return powerSum;
            };

            methods.getHeroCardsCount = function () {
                var heroCount = 0;
                angular.forEach(data.create.cards, function (card) {
                    if (gwintTypeService.methods.hasType(card.Type, 'Hero')) {
                        heroCount += card.Count;
                    }
                });
                return heroCount;
            };

            if (!data.initialized) {
                data.factions = gwintFactionService.factions;
                methods.getCards();
                methods.getDecks();
                data.initialized = true;
            }
        });
})();