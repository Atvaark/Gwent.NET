﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Extensions;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class PlayCardCommand : Command
    {
        public long CardId { get; set; }

        public long? TargetCardId { get; set; }

        public GwintSlot Slot { get; set; }

        public override void Execute(Game game)
        {
            RoundState state = game.State as RoundState;
            if (state == null)
            {
                throw new CommandException();
            }

            Player sender = game.GetPlayerByUserId(SenderUserId);
            Player opponent = game.GetOpponentPlayerByUserId(SenderUserId);
            if (sender == null || opponent == null)
            {
                throw new CommandException();
            }

            if (sender.IsTurn == false)
            {
                throw new CommandException();
            }

            PlayerCard card = sender.HandCards.FirstOrDefault(c => c.Card.Id == CardId);
            if (card == null)
            {
                throw new CommandException();
            }

            if (!ValidateSlot(card, Slot))
            {
                throw new CommandException();
            }

            PlayCard(card, sender, opponent, Slot, TargetCardId);
            // TODO: Calculate the effective power of each card, of each row and of each player
            
            sender.IsTurn = false;
            opponent.IsTurn = true;

            var turnEvents = game.Players.Select(player => new TurnEvent(player.User.Id)
            {
                Game = game.ToPersonalizedDto(player.User.Id)
            });

            Events.AddRange(turnEvents);
        }


        private bool ValidateSlot(PlayerCard playerCard, GwintSlot slot)
        {
            Card card = playerCard.Card;
            var types = card.Types;
            if (types.HasFlag(GwintType.GlobalEffect))
            {
                return true;
            }

            switch (slot)
            {
                case GwintSlot.None:
                    return false;
                case GwintSlot.Melee:
                    return types.HasFlag(GwintType.Creature | GwintType.Melee) || types.HasFlag(GwintType.Spell);
                case GwintSlot.Ranged:
                    return types.HasFlag(GwintType.Creature | GwintType.Ranged) || types.HasFlag(GwintType.Spell);
                case GwintSlot.Siege:
                    return types.HasFlag(GwintType.Creature | GwintType.Siege) || types.HasFlag(GwintType.Spell);
                case GwintSlot.MeleeModifier:
                case GwintSlot.RangedModifier:
                case GwintSlot.SiegeModifier:
                    return types.HasFlag(GwintType.RowModifier);
                case GwintSlot.Weather:
                    return types.HasFlag(GwintType.Weather);
                default:
                    throw new CommandException();
            }
        }


        private void PlayCard(PlayerCard playerCard, Player sender, Player opponent, GwintSlot slot, long? targetCardId)
        {
            Card card = playerCard.Card;

            if (card.Types.HasFlag(GwintType.Creature))
            {
                PlayCreatureCard(playerCard, sender, opponent, slot, targetCardId);
            }
            else if (card.Types.HasFlag(GwintType.Weather))
            {
                PlayWeatherCard(playerCard, sender, opponent, slot);
            }
            else if (card.Types.HasFlag(GwintType.Spell))
            {
                PlaySpellCard(playerCard, sender, opponent, slot, targetCardId);
            }
            else if (card.Types.HasFlag(GwintType.RowModifier))
            {
                PlayRowModifierCard(playerCard, sender, opponent, slot);
            }
            else if (card.Types.HasFlag(GwintType.GlobalEffect))
            {
                PlayGlobalEffectCard(playerCard, sender, opponent);
            }
            else
            {
                throw new CommandException();
            }

            sender.HandCards.Remove(playerCard);
        }

        private void PlayCreatureCard(PlayerCard playerCard, Player sender, Player opponent, GwintSlot slot, long? targetCardId)
        {
            Card card = playerCard.Card;

            SpawnCreature(playerCard, sender, opponent, slot);

            switch (card.Effect)
            {
                case GwintEffect.MeleeScorch:
                    ScorchRowCards(opponent, GwintSlot.Melee, Constants.ScorchThresholdMelee);
                    break;
                case GwintEffect.SiegScorch:
                    ScorchRowCards(opponent, GwintSlot.Siege, Constants.ScorchThresholdSiege);
                    break;
                case GwintEffect.Nurse:
                    NurseCard(sender, opponent, targetCardId);
                    break;
                case GwintEffect.Draw2:
                    DrawCards(sender, 2);
                    break;
                case GwintEffect.SummonClones:
                    SummonCardClones(playerCard, sender, opponent);
                    break;
            }
        }

        private void NurseCard(Player sender, Player opponent, long? targetCardId)
        {
            if (!targetCardId.HasValue)
            {
                throw new CommandException();
            }

            var nursedCard = sender.GraveyardCards.FirstOrDefault(c => c.Card.Id == targetCardId.Value && c.Card.Types.HasFlag(GwintType.Creature));
            if (nursedCard == null)
            {
                throw new CommandException();
            }

            GwintSlot slot = GetDefaultCreatureSlot(nursedCard.Card);

            // TODO: Check if nursed nurses can nurse. If they can then use a list of target card ids instead of a single targetCardId.
            PlayCreatureCard(nursedCard, sender, opponent, slot, null);
        }

        private GwintSlot GetDefaultCreatureSlot(Card card)
        {
            if (card.Types.HasFlag(GwintType.Melee))
            {
                return GwintSlot.Melee;
            }
            if (card.Types.HasFlag(GwintType.Ranged))
            {
                return GwintSlot.Ranged;
            }
            if (card.Types.HasFlag(GwintType.Siege))
            {
                return GwintSlot.Siege;
            }
            return GwintSlot.None;
        }

        private void ScorchRowCards(Player player, GwintSlot slot, int scorchThreshold)
        {
            var rowPower = player.CardSlots
                .Where(s => s.Slot == slot)
                .Sum(s => s.EffectivePower);
            if (rowPower >= scorchThreshold)
            {
                var scorchTargets = player.CardSlots
                    .Where(s => s.Slot == slot && !s.Card.Types.HasFlag(GwintType.Hero))
                    .GroupBy(s => s.EffectivePower)
                    .OrderByDescending(g => g.Key)
                    .FirstOrDefault();
                if (scorchTargets != null)
                {
                    foreach (var scorchTarget in scorchTargets)
                    {
                        player.CardSlots.Remove(scorchTarget);
                        player.GraveyardCards.Add(scorchTarget.ToPlayerCard());
                    }
                }
            }
        }

        private void SpawnCreature(PlayerCard playerCard, Player sender, Player opponent, GwintSlot slot)
        {
            Card card = playerCard.Card;

            Player creaturePlayer = card.Types.HasFlag(GwintType.Spy) ? opponent : sender;
            switch (slot)
            {
                case GwintSlot.Melee:
                case GwintSlot.Ranged:
                case GwintSlot.Siege:
                    var playerCardSlot = new PlayerCardSlot
                    {
                        Card = card,
                        Slot = slot
                    };
                    creaturePlayer.CardSlots.Add(playerCardSlot);
                    break;
                default:
                    throw new CommandException();
            }
        }

        private void DrawCards(Player player, int count)
        {
            var drawCount = Math.Min(player.DeckCards.Count, count);
            var drawnCards = player.DeckCards.Take(drawCount).ToList();
            foreach (var drawnCard in drawnCards)
            {
                player.DeckCards.Remove(drawnCard);
                player.HandCards.Add(drawnCard);
            }
        }

        private void SummonCardClones(PlayerCard playerCard, Player sender, Player opponent)
        {
            Card card = playerCard.Card;
            var summonCardIds = card.SummonFlags.Select(s => s.SummonCard.Id).ToList();
            SummonCloneCardsInSet(summonCardIds, sender, opponent, sender.DeckCards);
            SummonCloneCardsInSet(summonCardIds, sender, opponent, sender.HandCards);
        }

        private void SummonCloneCardsInSet(List<long> summonCardIds, Player sender, Player opponent, ICollection<PlayerCard> cards)
        {
            var summonHandCards = cards.Where(h => summonCardIds.Contains(h.Card.Id)).ToList();
            foreach (var summonHandCard in summonHandCards)
            {
                cards.Remove(summonHandCard);
                GwintSlot slot = GetDefaultCreatureSlot(summonHandCard.Card);
                SpawnCreature(summonHandCard, sender, opponent, slot);
            }
        }

        private void PlayWeatherCard(PlayerCard playerCard, Player sender, Player opponent, GwintSlot slot)
        {
            Card card = playerCard.Card;

            if (slot != GwintSlot.Weather)
            {
                throw new CommandException();
            }
            if (card.Effect.HasFlag(GwintEffect.ClearSky))
            {
                ClearSlot(sender, GwintSlot.Weather);
                ClearSlot(opponent, GwintSlot.Weather);
                sender.GraveyardCards.Add(playerCard);
            }
            else
            {
                SpawnWeatherCard(playerCard, sender, opponent);
            }
        }

        private void SpawnWeatherCard(PlayerCard playerCard, Player sender, Player opponent)
        {
            Card card = playerCard.Card;

            ClearSlotEffect(sender, GwintSlot.Weather, card.Effect);
            ClearSlotEffect(opponent, GwintSlot.Weather, card.Effect);

            var playerCardSlot = new PlayerCardSlot
            {
                Card = card,
                Slot = GwintSlot.Weather
            };
            sender.CardSlots.Add(playerCardSlot);
        }

        private static void ClearSlotEffect(Player player, GwintSlot slot, GwintEffect effect)
        {
            var existingWeatherCard = player.CardSlots
                .FirstOrDefault(s => s.Slot == slot && s.Card.Effect == effect);
            if (existingWeatherCard != null)
            {
                player.CardSlots.Remove(existingWeatherCard);
                player.GraveyardCards.Add(existingWeatherCard.ToPlayerCard());
            }
        }

        private void ClearSlot(Player player, GwintSlot slot)
        {
            var cardSlots = player.CardSlots.Where(s => s.Slot == slot).ToList();
            foreach (var cardSlot in cardSlots)
            {
                player.CardSlots.Remove(cardSlot);
                player.GraveyardCards.Add(cardSlot.ToPlayerCard());
            }
        }

        private void PlaySpellCard(PlayerCard playerCard, Player sender, Player opponent, GwintSlot slot, long? targetCardId)
        {
            Card card = playerCard.Card;

            switch (card.Effect)
            {
                case GwintEffect.UnsummonDummy:
                    PlayUnsummonDummy(sender, playerCard, slot, targetCardId);
                    break;
                default:
                    throw new CommandException();
            }
        }

        private void PlayUnsummonDummy(Player player, PlayerCard playerCard, GwintSlot slot, long? targetCardId)
        {
            Card card = playerCard.Card;

            if (!targetCardId.HasValue)
            {
                throw new CommandException();
            }

            PlayerCardSlot cardSlot = GetUnsummonTargetSlot(player, slot, targetCardId.Value);
            player.HandCards.Add(cardSlot.ToPlayerCard());
            cardSlot.Card = card;
        }

        private static PlayerCardSlot GetUnsummonTargetSlot(Player player, GwintSlot slot, long targetCardId)
        {
            switch (slot)
            {
                case GwintSlot.Melee:
                case GwintSlot.Ranged:
                case GwintSlot.Siege:
                    var cardSlot = player.CardSlots.FirstOrDefault(s => s.Slot == slot && s.Card.Id == targetCardId);
                    if (cardSlot == null)
                    {
                        throw new CommandException();
                    }
                    return cardSlot;
                default:
                    throw new CommandException();
            }
        }

        private void PlayGlobalEffectCard(PlayerCard playerCard, Player sender, Player opponent)
        {
            Card card = playerCard.Card;

            switch (card.Effect)
            {
                case GwintEffect.Scorch:
                    PlayScorch(sender, opponent, playerCard);
                    break;
                default:
                    throw new CommandException();

            }
        }
        private void PlayScorch(Player sender, Player opponent, PlayerCard playerCard)
        {
            var senderSlots = SelectHighestPowerCardGroup(sender);
            var opponentSlots = SelectHighestPowerCardGroup(opponent);

            if (senderSlots == null && opponentSlots == null)
            {
                throw new CommandException();
            }

            if (senderSlots != null)
            {
                if (opponentSlots != null)
                {
                    if (senderSlots.Key == opponentSlots.Key)
                    {
                        ScorchCardSlots(sender, senderSlots);
                        ScorchCardSlots(opponent, opponentSlots);
                    }
                    else if (senderSlots.Key < opponentSlots.Key)
                    {
                        ScorchCardSlots(opponent, opponentSlots);
                    }
                    else
                    {
                        ScorchCardSlots(sender, senderSlots);
                    }
                }
                else
                {
                    ScorchCardSlots(sender, senderSlots);
                }
            }
            else
            {
                ScorchCardSlots(opponent, opponentSlots);
            }

            sender.GraveyardCards.Add(playerCard);
        }

        private IGrouping<int, PlayerCardSlot> SelectHighestPowerCardGroup(Player player)
        {
            return player.CardSlots
                .Where(s => s.Card.Types.HasFlag(GwintType.Creature) && !s.Card.Types.HasFlag(GwintType.Hero))
                .GroupBy(s => s.EffectivePower)
                .OrderByDescending(g => g.Key)
                .FirstOrDefault();
        }

        private void ScorchCardSlots(Player player, IEnumerable<PlayerCardSlot> cardSlots)
        {
            foreach (var cardSlot in cardSlots)
            {
                player.CardSlots.Remove(cardSlot);
                player.GraveyardCards.Add(cardSlot.ToPlayerCard());
            }
        }

        private void PlayRowModifierCard(PlayerCard playerCard, Player sender, Player opponent, GwintSlot slot)
        {
            Card card = playerCard.Card;

            switch (card.Effect)
            {
                case GwintEffect.Horn:
                    if (!ValidateRowModifierCard(card, slot))
                    {
                        throw new CommandException();
                    }

                    ClearSlotEffect(sender, slot, card.Effect);
                    var rowModifierSlot = new PlayerCardSlot
                    {
                        Card = card,
                        Slot = slot
                    };
                    sender.CardSlots.Add(rowModifierSlot);
                    break;
                default:
                    throw new CommandException();
            }
        }

        private bool ValidateRowModifierCard(Card card, GwintSlot slot)
        {
            return card.Types.HasFlag(GwintType.Melee) && slot == GwintSlot.MeleeModifier 
                || card.Types.HasFlag(GwintType.Ranged) && slot == GwintSlot.RangedModifier 
                || card.Types.HasFlag(GwintType.Siege) && slot == GwintSlot.SiegeModifier;
        }
    }
}