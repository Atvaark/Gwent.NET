using System;
using System.Diagnostics;
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
            return card.EffectFlags.Aggregate(GwintEffect.EffectNone, (current, effectFlag) => current | effectFlag.Name);
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
                FactionIndex = GwentFaction.Neutral,
                Type = card.GetGwintType(),
                Effect = card.GetGwintEffect(),
                SummonFlags = card.SummonFlags.Select(s => s.Id).ToList(),
                IsBattleKing = card.IsBattleKing
            };
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

        public static DeckDto ToDto(this Deck deck)
        {
            return new DeckDto
            {
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
                StateType = game.State.GetType().Name,
                Participants = game.Participants.Select(p => p.ToDto()).ToList()
            };
        }

        public static ParticipantDto ToDto(this Participant participant)
        {
            return new ParticipantDto
            {
                User = participant.User.Id,
                Deck = participant.Deck.Id,
                IsOwner = participant.IsOwner,
                Lives = participant.Lives,
                Draws = participant.Draws,
                DeckCardCount = participant.DeckCards.Count,
                DisposedCards = participant.DisposedCards.Select(c => c.Index).ToList(),
                CloseCombatCards = participant.CloseCombatCards.Select(c => c.Index).ToList(),
                RangeCards = participant.RangeCards.Select(c => c.Index).ToList(),
                SiegeCards = participant.RangeCards.Select(c => c.Index).ToList()
            };
        }
    }
}