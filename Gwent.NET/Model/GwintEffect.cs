using System;
using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    [Flags]
    public enum GwintEffect : long  
    {
        [XmlEnum("EFFECT_NONE")]
        None = 0,

        Backstab = 1,

        MoraleBoost = 2,

        Ambush = 3,

        ToughSkin = 4,

        Bin2 = 5,

        Bin3 = 6,

        [XmlEnum("CP_MELEE_SCORCH")]
        MeleeScorch = 7,

        [XmlEnum("CP_11TH_CARD")]
        EleventhCard = 8,

        [XmlEnum("CP_CLEAR_WEATHER")]
        ClearWeather = 9,

        [XmlEnum("CP_PICK_WEATHER_CARD")]
        PickWeather = 10,

        [XmlEnum("CP_PICK_RAIN_CARD")]
        PickRain = 11,

        [XmlEnum("CP_PICK_FOG_CARD")]
        PickFog = 12,

        [XmlEnum("CP_PICK_FROST_CARD")]
        PickFrost = 13,

        [XmlEnum("CP_VIEW_3_ENEMY_CARDS")]
        View3Enemy = 14,

        [XmlEnum("CP_RESURECT_CARD")]
        Resurrect = 15,

        [XmlEnum("CP_RESURECT_FROM_ENEMY")]
        ResurrectEnemy = 16,

        [XmlEnum("CP_BIN2_PICK1")]
        Bin2Pick1 = 17,

        [XmlEnum("CP_MELEE_HORN")]
        MeleeHorn = 18,

        [XmlEnum("CP_RANGE_HORN")]
        RangeHorn = 19,

        [XmlEnum("CP_SIEGE_HORN")]
        SiegeHorn = 20,

        [XmlEnum("CP_SIEGE_SCORCH")]
        SiegScorch = 21,

        [XmlEnum("CP_COUNTER_KING_ABLILITY")]
        CounterKing = 22,

        [XmlEnum("EFFECT_MELEE")]
        Melee = 23,

        [XmlEnum("EFFECT_RANGED")]
        Ranged = 24,

        [XmlEnum("EFFECT_SIEGE")]
        Siege = 25,

        [XmlEnum("EFFECT_UNSUMMON_DUMMY")]
        UnsummonDummy = 26,

        [XmlEnum("EFFECT_HORN")]
        Horn = 27,

        [XmlEnum("EFFECT_DRAW")]
        Draw = 28,

        [XmlEnum("EFFECT_SCORCH")]
        Scorch = 29,

        [XmlEnum("EFFECT_CLEAR_SKY")]
        ClearSky = 30,

        [XmlEnum("EFFECT_SUMMON_CLONES")]
        SummonClones = 31,

        [XmlEnum("EFFECT_IMPROVE_NEIGHBOURS")]
        ImproveNeighbours = 32,

        [XmlEnum("EFFECT_NURSE")]
        Nurse = 33,

        [XmlEnum("EFFECT_DRAW_X2")]
        Draw2 = 34,

        [XmlEnum("EFFECT_SAME_TYPE_MORALE")]
        SameTypeMorale = 35
    }
}