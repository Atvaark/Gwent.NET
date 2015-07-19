using System.Collections.Generic;
using System.Linq;
using Gwent.NET.DTOs;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Extensions
{
    public static class ModelExtensions
    {
        public static GwintType GetGwintTypes(this Card card)
        {
            return card.TypeFlags.Aggregate(GwintType.None, (current, typeFlag) => current | typeFlag.Name);
        }

        public static GwintEffect GetGwintEffects(this Card card)
        {
            return card.EffectFlags.Aggregate(GwintEffect.None, (current, effectFlag) => effectFlag.Name);
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
                Id = card.Id,
                Title = card.Title,
                Description = card.Description,
                Power = card.Power,
                Picture = card.Picture,
                Faction = card.FactionIndex,
                Type = card.Types,
                Effect = card.Effect,
                SummonFlags = card.SummonFlags.Select(s => s.SummonCardId).ToList(),
                IsBattleKing = card.IsBattleKing
            };
        }

        public static DeckDto ToDto(this Deck deck)
        {
            return new DeckDto
            {
                Id = deck.Id,
                Cards = deck.Cards.Select(c => c.Card.Id).ToList(),
                Faction = deck.Faction,
                BattleKingCard = deck.BattleKingCard.Card.Id,
                IsPrimaryDeck = deck.IsPrimaryDeck
            };
        }

        public static GameBrowseDto ToGameBrowseDto(this Game game)
        {
            return new GameBrowseDto
            {
                Id = game.Id,
                State = game.State.Name,
                PlayerCount = game.Players.Count
            };
        }

        public static GameDto ToPersonalizedDto(this Game game, long userId)
        {
            Dictionary<string, PlayerDto> players = new Dictionary<string, PlayerDto>();
            foreach (var player in game.Players)
            {
                if (player.User.Id == userId)
                {
                    players[Constants.PlayerKeySelf] = player.ToDto();
                }
                else
                {
                    var opponentDto = player.ToDto();
                    opponentDto.Cards[GwintSlot.Hand].Clear();
                    players[Constants.PlayerKeyOpponent] = opponentDto;
                }
            }

            return new GameDto
            {
                Id = game.Id,
                State = game.State.Name,
                Players = players
            };
        }

        public static PlayerDto ToDto(this Player player)
        {
            Dictionary<GwintSlot, List<PlayerCardDto>> cards = player.CardSlots
                .GroupBy(c => c.Slot)
                .ToDictionary(g => g.Key, g => g.Select(c => c.ToDto()).ToList());
            //cards.Add(GwintSlot.Deck, player.DeckCards.Select(c => c.ToDto()).ToList());
            cards.Add(GwintSlot.Hand, player.HandCards.Select(c => c.ToDto()).ToList());
            cards.Add(GwintSlot.Graveyard, player.GraveyardCards.Select(c => c.ToDto()).ToList());

            return new PlayerDto
            {
                Id = player.User.Id,
                Name = player.User.Name,
                IsLobbyOwner = player.IsOwner,

                IsPassing = player.IsPassing,
                IsTurn = player.IsTurn,
                Lives = player.Lives,

                Faction = player.Faction,
                BattleKingCard = player.BattleKingCard == null ? new long?() : player.BattleKingCard.Card.Id,
                CanUseBattleKingCard = player.CanUseBattleKingCard,

                HandCardCount = player.HandCards.Count,
                DeckCardCount = player.DeckCards.Count,

                Cards = cards
            };
        }

        public static PlayerCardDto ToDto(this PlayerCard playerCard)
        {
            return new PlayerCardDto
            {
                Id = playerCard.Id,
                CardId = playerCard.Card.Id
            };
        }
        
        public static PlayerCardDto ToDto(this PlayerCardSlot playerCardSlot)
        {
            return new PlayerCardDto
            {
                Id = playerCardSlot.Id,
                CardId = playerCardSlot.Card.Id,
                IsActive = true // HACK: This is to differentiate between the ids of PlayarCard and PlayerCardSlot.
            };
        }

        public static PlayerCard ToPlayerCard(this DeckCard deckCard)
        {
            return new PlayerCard { Card = deckCard.Card };
        }

        public static PlayerCard ToPlayerCard(this PlayerCardSlot playerCardSlot)
        {
            return new PlayerCard { Card = playerCardSlot.Card };
        }

        public static PlayerCard ToPlayerCard(this Card card)
        {
            return new PlayerCard { Card = card };
        }

        public static DeckCard ToDeckCard(this Card card)
        {
            return new DeckCard { Card = card };
        }
    }
}