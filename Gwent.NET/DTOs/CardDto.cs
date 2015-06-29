using System.Collections.Generic;
using Gwent.NET.Model;

namespace Gwent.NET.DTOs
{
    public class CardDto
    {
        public int Index { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Power { get; set; }

        public string Picture { get; set; }
        
        public GwintFaction Faction { get; set; }

        public GwintType Type { get; set; }

        public GwintEffect Effect { get; set; }

        public List<int> SummonFlags { get; set; }

        public bool IsBattleKing { get; set; }
    }
}