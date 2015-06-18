using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class PlayCardCommand : Command
    {
        public int CardId { get; set; }
        public int? ResurrectCardId { get; set; }
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

            sender.IsTurn = false;
            opponent.IsTurn = true;

            // TODO: Implement card playing
        }
    }
}