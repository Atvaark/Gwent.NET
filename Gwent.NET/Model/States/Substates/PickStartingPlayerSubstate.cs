namespace Gwent.NET.Model.States.Substates
{
    public class PickStartingPlayerSubstate : Substate
    {
        public bool CanPickStartingPlayer { get; set; }
        public int? StartingPlayerUserId { get; set; }
    }
}