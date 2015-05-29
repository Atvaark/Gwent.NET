using System;
using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    [Flags]
    public enum GwintType
    {

        [XmlEnum("TYPE_NONE")]
        None = 0,

        [XmlEnum("TYPE_GLOBAL_EFFECT")]
        GlobalEffect = 1 << 0,

        [XmlEnum("TYPE_FRIENDLY_EFFECT")]
        FriendlyEffect = 1 << 1,

        [XmlEnum("TYPE_OFFENSIVE_EFFECT")]
        OffensiveEffect = 1 << 2,

        [XmlEnum("TYPE_ROW_MODIFIER")]
        RowModifier = 1 << 3,

        [XmlEnum("TYPE_SPELL")]
        Spell = 1 << 4,

        [XmlEnum("TYPE_WEATHER")]
        Weather = 1 << 5,

        [XmlEnum("TYPE_CREATURE")]
        Creature = 1 << 6,

        [XmlEnum("TYPE_MELEE")]
        Melee = 1 << 7,

        [XmlEnum("TYPE_RANGED")]
        Ranged = 1 << 8,

        [XmlEnum("TYPE_SIEGE")]
        Siege = 1 << 9,

        [XmlEnum("TYPE_HERO")]
        Hero = 1 << 10,

        [XmlEnum("TYPE_SPY")]
        Spy = 1 << 11
    }
}