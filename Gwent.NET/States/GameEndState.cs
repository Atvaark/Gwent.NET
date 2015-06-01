namespace Gwent.NET.States
{
    public class GameEndState : State
    {
        public override bool IsOver
        {
            get { return true; }
        }
    }
}