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

            opponent.IsTurn = true;

            // TODO: Implement battle king card abilities
        }
    }
}
