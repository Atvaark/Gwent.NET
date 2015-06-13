using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;

namespace Gwent.NET.Model.States
{
    public class RoundState : State
    {


        public override IEnumerable<Event> Initialize(Game game)
        {
            var roundStarter = game.Players.First(g => g.IsRoundStarter);
            roundStarter.IsTurn = true;
            yield return new TurnEvent(game.GetAllUserIds());
        }
    }
}