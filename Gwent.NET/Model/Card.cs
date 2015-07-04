using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Gwent.NET.Model.Enums;
using Gwent.NET.Model.Serialization;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("power")]
        public int Power { get; set; }

        [XmlAttribute("picture")]
        public string Picture { get; set; }

        [XmlAttribute("faction_index")]
        public GwintFaction FactionIndex { get; set; }

        [XmlArray("type_flags")]
        [XmlArrayItem("flag")]
        [JsonIgnore]
        [NotMapped]
        public List<GwintTypeFlag> TypeFlags { get; set; }

        [XmlIgnore]
        public GwintType Types { get; set; }
        
        [XmlArray("effect_flags")]
        [XmlArrayItem("flag")]
        [JsonIgnore]
        [NotMapped]
        public List<GwintEffectFlag> EffectFlags { get; set; }

        [XmlIgnore]
        public GwintEffect Effect { get; set; }
        
        [XmlArray("summonFlags", IsNullable = true)]
        [XmlArrayItem("card")]
        [InverseProperty("SummonerCard")]
        public List<GwintSummonFlag> SummonFlags { get; set; }

        [XmlIgnore]
        public bool IsBattleKing { get; set; }

        [XmlIgnore]
        [InverseProperty("Card")]
        public virtual ICollection<DeckCard> DeckCards { get; set; }

        [XmlIgnore]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}