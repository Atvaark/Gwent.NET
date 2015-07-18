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
                }, function (msg) {
                    $log.error('Unable to retrieve cards.');
                    data.cards = [];
                });
            };

            methods.getDecks = function () {
                backendService.methods.getDecks().then(function (decks) {
                    $log.info('Decks updated.');
                    data.decks = decks;
                }, function (msg) {
                    $log.error('Unable to retrieve decks.');
                    data.decks = [];
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
                var deckCard = methods.findDeckCard(card.index);
                if (deckCard) {
                    deckCard.count = deckCard.count + 1;
                    return;
                }
                deckCard = {
                    index: card.index,
                    title: card.title,
                    description: card.description,
                    power: card.power,
                    picture: card.picture,
                    faction: card.faction,
                    type: card.type,
                    effect: card.effect,
                    summonFlags: card.summonFlags,
                    isBattleKing: card.isBattleKing,
                    count: 1
                };
                data.create.cards.push(deckCard);
            };

            methods.removeDeckCard = function (cardIndex) {
                data.create.cards.splice(cardIndex, 1);
            };

            methods.findDeckCard = function (cardIndex) {
                var foundCard = null;
                // TODO: Use a simple for loop
                angular.forEach(data.create.cards, function (deckCard) {
                    if (deckCard.index === cardIndex) {
                        foundCard = deckCard;
                        return;
                    }
                });
                return foundCard;
            };

            methods.createDeck = function () {
                var deckDto = {
                    Cards: [],
                    faction: data.create.faction,
                    BattleKingCard: data.create.battleKing
                };
                angular.forEach(data.create.cards, function (card) {
                    for (var i = 0; i < card.count; i++) {
                        deckDto.Cards.push(card.index);
                    }
                });
                backendService.methods.createDeck(deckDto).then(function (response) {
                    $log.info('Deck created.');
                    methods.showDecks();
                }, function (msg) {
                    $log.error('Unable to create deck.');
                });
            };

            methods.getTotalCardsInDeck = function () {
                var count = 0;
                angular.forEach(data.create.cards, function(card) {
                    count += card.count;
                });
                return count;
            };

            methods.getUnitCardsCount = function () {
                var unitCardCount = 0;
                angular.forEach(data.create.cards, function (card) {
                    if (gwintTypeService.hasType(card.type, 'Creature')) {
                        unitCardCount += card.count;
                    }
                });
                return unitCardCount;
            };

            methods.getSpecialCardsCount = function () {
                var specialCardCount = 0;
                angular.forEach(data.create.cards, function (card) {
                    if (!gwintTypeService.hasType(card.type, 'Creature')) {
                        specialCardCount += card.count;
                    }
                });
                return specialCardCount;
            };
            
            methods.getTotalUnitCardStrength = function () {
                var powerSum = 0;
                angular.forEach(data.create.cards, function (card) {
                    powerSum += card.power * card.count;
                });
                return powerSum;
            };

            methods.getHeroCardsCount = function () {
                var heroCount = 0;
                angular.forEach(data.create.cards, function (card) {
                    if (gwintTypeService.hasType(card.type, 'Hero')) {
                        heroCount += card.count;
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