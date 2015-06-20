using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;

namespace Gwent.NET.Model.States
{
    public class RoundState : State
    {
        public override string Name
        {
            get { return "Round"; }
        }

        public override IEnumerable<Event> Initialize(Game game)
        {
            var roundStarter = game.Players.First(g => g.IsRoundStarter);
            roundStarter.IsTurn = true;

            foreach (var player in game.Players)
            {
                player.Lives = Constants.InitialLifeCount;
            }

            return game.Players.Select(player => new TurnEvent(player.User.Id)
            {
                Game = game.ToPersonalizedDto(player.User.Id)
            });

        }
    }
}