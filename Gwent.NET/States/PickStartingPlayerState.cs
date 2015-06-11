using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.States.Substates;

namespace Gwent.NET.States
{
    public class PickStartingPlayerState : State
    {
        public List<PickStartingPlayerSubstate> Substates { get; set; }

        public PickStartingPlayerState()
        {
            Substates = new List<PickStartingPlayerSubstate>();
        }

        public override IEnumerable<Event> Initialize(Game game)
        {
            Substates.AddRange(game.Players.Select(p => new PickStartingPlayerSubstate
            {
                UserId = p.User.Id,
                CanPickStartingPlayer = p.Deck.Faction == GwentFaction.Scoiatael
            }));
            yield break;
        }
    }
}
