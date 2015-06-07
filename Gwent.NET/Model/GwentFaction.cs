using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public enum GwentFaction
    {
        [XmlEnum("F_NEUTRAL")]
        Neutral = 0,
        
        [XmlEnum("F_NO_MANS_LAND")]
        NoMansLand = 1,

        [XmlEnum("F_NILFGAARD")]
        Nilfgaard = 2,

        [XmlEnum("F_NORTHERN_KINGDOM")]
        NorthernKingdom = 3,
        
        [XmlEnum("F_SCOIATAEL")]
        Scoiatael = 4
    }
}