using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model.States.Substates;

namespace Gwent.NET.Model.States
{
    public class PickStartingPlayerState : State
    {
        public virtual ICollection<PickStartingPlayerSubstate> Substates { get; set; }

        public PickStartingPlayerState()
        {
            Substates = new HashSet<PickStartingPlayerSubstate>();
        }

        public override IEnumerable<Event> Initialize(Game game)
        {
            var substates = game.Players
                .Select(p => new PickStartingPlayerSubstate
                {
                    UserId = p.User.Id,
                    CanPickStartingPlayer = p.Deck.Faction == GwentFaction.Scoiatael
                });
            Substates.AddRange(substates);
            yield break;
        }
    }
}
