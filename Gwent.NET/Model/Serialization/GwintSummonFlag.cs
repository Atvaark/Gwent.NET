using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Gwent.NET.Model.Serialization
{
    public class GwintSummonFlag
    {
        [XmlIgnore]
        [Key, Column(Order = 0)]
        public int SummonerCardId { get; set; }

        [XmlAttribute("id")]
        [Key, Column(Order = 1)]
        public int SummonCardId { get; set; }

        [XmlIgnore]
        [ForeignKey("SummonCardId")]
        public virtual Card SummonCard { get; set; }

        [XmlIgnore]
        [InverseProperty("SummonFlags")]
        [ForeignKey("SummonerCardId")]
        public virtual Card SummonerCard { get; set; }

        [XmlIgnore]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}