using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Gwent.NET.Model
{
    public class Card
    {
        public Card()
        {
            TypeFlags = new List<GwintTypeFlag>();
            EffectFlags = new List<GwintEffectFlag>();
            SummonFlags = new List<GwintSummonFlag>();
        }

        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("power")]
        public int Power { get; set; }

        [XmlAttribute("picture")]
        public string Picture { get; set; }

        [XmlAttribute("faction_index")]
        public GwentFaction FactionIndex { get; set; }

        [XmlArray("type_flags")]
        [XmlArrayItem("flag")]
        [JsonIgnore]
        public List<GwintTypeFlag> TypeFlags { get; set; }
        
        [XmlArray("effect_flags")]
        [XmlArrayItem("flag")]
        [JsonIgnore]
        public List<GwintEffectFlag> EffectFlags { get; set; }
        
        [XmlArray("summonFlags", IsNullable = true)]
        [XmlArrayItem("card")]
        public List<GwintSummonFlag> SummonFlags { get; set; }

        public bool IsBattleKing { get; set; }
    }
}