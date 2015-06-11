namespace Gwent.NET.States.Substates
{
    public class PickStartingPlayerSubstate
    {
        public int UserId { get; set; }
        public bool CanPickStartingPlayer { get; set; }
        public int? StartingPlayerId { get; set; }
    }
}