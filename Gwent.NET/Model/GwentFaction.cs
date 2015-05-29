using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public enum GwentFaction
    {
        [XmlEnum("F_NEUTRAL")]
        Neutral,

        [XmlEnum("F_NORTHERN_KINGDOM")]
        NorthernKingdom,

        [XmlEnum("F_NILFGAARD")]
        Nilfgaard,

        [XmlEnum("F_SCOIATAEL")]
        Scoiatael,

        [XmlEnum("F_NO_MANS_LAND")]
        NoMansLand 
    }
}