using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public class Card
    {
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
        public List<GwintTypeFlag> TypeFlags { get; set; }

        [XmlArray("effect_flags")]
        [XmlArrayItem("flag")]
        public List<GwintEffectFlag> EffectFlags { get; set; }

        [XmlArray("summonFlags", IsNullable = true)]
        [XmlArrayItem("card")]
        public List<GwintSummonFlag> SummonFlags { get; set; }
    }
}