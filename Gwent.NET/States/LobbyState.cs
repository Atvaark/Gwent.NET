namespace Gwent.NET.States
{
    public class LobbyState : State
    {
        public override bool IsJoinable
        {
            get { return true; }
        }
    }
}