namespace Gwent.NET.States
{
    public abstract class State
    {
        public virtual bool IsOver
        {
            get { return false; }
        }

        public virtual bool IsJoinable
        {
            get { return false; }
        }

        public string Name
        {
            get { return GetType().Name; }
        }
    }
}