using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class UseBattleKingCardCommand : Command
    {
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

            if (!sender.IsTurn)
            {
                throw new CommandException();
            }

            if (!sender.CanUseBattleKingCard)
            {
                throw new CommandException();
            }

            // TODO: Implement battle king card abilities
            switch (sender.BattleKingCard.Effect)
            {
                case GwintEffect.MeleeScorch:
                    // TODO: Remove enemy non-hero unit with highest power if row power is >= 10
                    break;
                case GwintEffect.ClearWeather:
                    // TODO: Remove any weather cards from the game
                    break;
                case GwintEffect.PickWeather:
                    // TODO: Prompt player to pick on of the weather cards in his deck
                    break;
                case GwintEffect.PickRain:
                    // TODO: Pick a rain card from the deck
                    break;
                case GwintEffect.PickFog:
                    // TODO: Pick a fog card from the deck
                    break;
                case GwintEffect.PickFrost:
                    // TODO: Pick a frost card from the deck
                    break;
                case GwintEffect.View3Enemy:
                    // TODO: Display 3 random enemy cards
                    break;
                case GwintEffect.Resurrect:
                    // TODO: Resurrect a card from the graveyard to hand
                    break;
                case GwintEffect.ResurrectEnemy:
                    // TODO: Resurrect a card from the enemy graveyard to hand
                    break;
                case GwintEffect.Bin2Pick1:
                    // TODO: Discard one card and pick one from the deck.
                    break;
                case GwintEffect.MeleeHorn:
                    // TODO: Double the strength of own melee row.
                    break;
                case GwintEffect.RangeHorn:
                    // TODO: Double the strength of own ranged row.
                    break;
                case GwintEffect.SiegeHorn:
                    // TODO: Double the strength of own siege row.
                    break;
                case GwintEffect.SiegScorch:
                    // TODO: Remove enemy non-hero siege unit with highest power if row power is >= 10
                    break;
                case GwintEffect.CounterKing:
                    // TODO: Implement no fun allowed / if one player starts with it set the CanUseBattleKingCard to false right at the beginning
                    break;
            }

            sender.CanUseBattleKingCard = false;
            opponent.IsTurn = true;


        }
    }
}
