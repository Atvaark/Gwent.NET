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
            foreach (var player in game.Players)
            {
                bool canPickStartingPlayer = player.Deck.Faction == GwentFaction.Scoiatael;
                player.IsTurn = canPickStartingPlayer;
                Substates.Add(new PickStartingPlayerSubstate
                {
                    UserId = player.User.Id,
                    CanPickStartingPlayer = canPickStartingPlayer
                });
            }
            yield break;
        }
    }
}
