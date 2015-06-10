using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.DTOs;
using Gwent.NET.Model;

namespace Gwent.NET
{
    public static class ExtensionMethods
    {
        public static GwintType GetGwintType(this Card card)
        {
            return card.TypeFlags.Aggregate(GwintType.None, (current, typeFlag) => current | typeFlag.Name);
        }

        public static GwintEffect GetGwintEffect(this Card card)
        {
            return card.EffectFlags.Aggregate(GwintEffect.None, (current, effectFlag) => current | effectFlag.Name);
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Picture = user.Picture
            };
        }

        public static CardDto ToDto(this Card card)
        {
            return new CardDto
            {
                Index = card.Index,
                Title = card.Title,
                Description = card.Description,
                Power = card.Power,
                Picture = card.Picture,
                Faction = card.FactionIndex,
                Type = card.GetGwintType(),
                Effect = card.GetGwintEffect(),
                SummonFlags = card.SummonFlags.Select(s => s.Id).ToList(),
                IsBattleKing = card.IsBattleKing
            };
        }

        public static DeckDto ToDto(this Deck deck)
        {
            return new DeckDto
            {
                Id = deck.Id,
                Cards = deck.Cards.Select(c => c.Index).ToList(),
                Faction = deck.Faction,
                BattleKingCard = deck.BattleKingCard.Index
            };
        }

        public static GameDto ToDto(this Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                State = game.State.Name,
                Players = game.Players.Select(p => p.ToDto()).ToList()
            };
        }

        public static PlayerDto ToDto(this Player player)
        {
            return new PlayerDto
            {
                User = player.User.Id,
                Deck = player.Deck == null ? 0 : player.Deck.Id, // TODO: Set the deck beforehand
                IsLobbyOwner = player.IsOwner,
                Lives = player.Lives,
                HandCardCount = player.HandCards.Count,
                DeckCardCount = player.DeckCards.Count,
                DisposedCards = player.DisposedCards.Select(c => c.Index).ToList(),
                CloseCombatCards = player.CloseCombatCards.Select(c => c.Index).ToList(),
                RangeCards = player.RangeCards.Select(c => c.Index).ToList(),
                SiegeCards = player.RangeCards.Select(c => c.Index).ToList()
            };
        }

        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, random.Next(i, list.Count));
        }

        private static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}