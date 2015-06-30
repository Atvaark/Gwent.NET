using System.Collections.Generic;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Test
{
    public static class TestGameFactory
    {
        public static Game CreateGame()
        {
            return CreateGame(
                new List<Card>(),
                new List<PlayerCardSlot>());
        }

        public static Game CreateGame(
            List<Card> player1HandCards,
            List<PlayerCardSlot> player1CardSlots)
        {
            return CreateGame(
                player1HandCards,
                player1CardSlots,
                new List<Card>());
        }

        public static Game CreateGame(
            List<Card> player1HandCards,
            List<PlayerCardSlot> player1CardSlots,
            List<Card> player1GraveyardCards)
        {
            return CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards,
                new List<PlayerCardSlot>());
        }


        public static Game CreateGame(
            List<Card> player1HandCards,
            List<PlayerCardSlot> player1CardSlots,
            List<Card> player1GraveyardCards,
            List<PlayerCardSlot> player2CardSlots
            )
        {
            return CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards,
                player2CardSlots,
                new List<Card>());
        }

        public static Game CreateGame(
            List<Card> player1HandCards,
            List<PlayerCardSlot> player1CardSlots,
            List<Card> player1GraveyardCards,
            List<PlayerCardSlot> player2CardSlots,
            List<Card> player2GraveyardCards
            )
        {
            var game = new Game
            {
                State = new RoundState(),
                Players =
                {
                    new Player
                    {
                        User = new User
                        {
                            Id = 1
                        },
                        HandCards = player1HandCards,
                        CardSlots = player1CardSlots,
                        GraveyardCards = player1GraveyardCards,
                        IsTurn = true
                    },
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        },
                        CardSlots = player2CardSlots,
                        GraveyardCards = player2GraveyardCards
                    }
                }
            };
            return game;
        }
    }
}