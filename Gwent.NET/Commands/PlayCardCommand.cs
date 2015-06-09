using Gwent.NET.Model;

namespace Gwent.NET.Commands
{
    public class PlayCardCommand : Command
    {
        public int CardId { get; set; }
        public GwintSlot Slot { get; set; }
    }
}