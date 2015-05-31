(function () {
    'use strict';

    angular.module('app.menu')
        .controller('DeckController', function ($scope, $state, $log, cardService, backendService, gwintFactionService) {
            var data = $scope.data = {
                initialized: false,
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
                $state.go('menu.main.deck.list');
            };

            methods.newDeck = function () {
                $state.go('menu.main.deck.create');
            };

            methods.addCard = function(index) {
                var card = data.cards[index];
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

            methods.findDeckCard = function(cardIndex) {
                var foundCard = null;
                angular.forEach(data.create.cards, function(deckCard) {
                    if (deckCard.Index === cardIndex) {
                        foundCard = deckCard;
                        return;
                    }
                });
                return foundCard;
            };

            methods.removeCard = function(cardIndex) {
                data.create.cards.splice(cardIndex, 1);
            };

            methods.createDeck = function () {
                // TODO: Transform and call the backendService
            };

            if (!data.initialized) {
                data.factions = gwintFactionService.factions;
                methods.getCards();
                methods.getDecks();
                data.initialized = true;
            }
        });
})();