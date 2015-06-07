using System;
using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    [Flags]
    public enum GwintEffect : long  
    {
        [XmlEnum("EFFECT_NONE")]
        None = 0,

        Backstab = 1 << 0,

        MoraleBoost = 1 << 1,

        Ambush = 1 << 2,

        ToughSkin = 1 << 3,

        Bin2 = 1 << 4,

        Bin3 = 1 << 5,

        [XmlEnum("CP_MELEE_SCORCH")]
        MeleeScorch = 1 << 6,

        [XmlEnum("CP_11TH_CARD")]
        EleventhCard = 1 << 7,

        [XmlEnum("CP_CLEAR_WEATHER")]
        ClearWeather = 1 << 8,

        [XmlEnum("CP_PICK_WEATHER_CARD")]
        PickWeather = 1 << 9,

        [XmlEnum("CP_PICK_RAIN_CARD")]
        PickRain = 1 << 10,

        [XmlEnum("CP_PICK_FOG_CARD")]
        PickFog = 1 << 11,

        [XmlEnum("CP_PICK_FROST_CARD")]
        PickFrost = 1 << 12,

        [XmlEnum("CP_VIEW_3_ENEMY_CARDS")]
        View3Enemy = 1 << 13,

        [XmlEnum("CP_RESURECT_CARD")]
        Resurrect = 1 << 14,

        [XmlEnum("CP_RESURECT_FROM_ENEMY")]
        ResurrectEnemy = 1 << 15,

        [XmlEnum("CP_BIN2_PICK1")]
        Bin2Pick1 = 1 << 16,

        [XmlEnum("CP_MELEE_HORN")]
        MeleeHorn = 1 << 17,

        [XmlEnum("CP_RANGE_HORN")]
        RangeHorn = 1 << 18,

        [XmlEnum("CP_SIEGE_HORN")]
        SiegeHorn = 1 << 19,

        [XmlEnum("CP_SIEGE_SCORCH")]
        SiegScorch = 1 << 20,

        [XmlEnum("CP_COUNTER_KING_ABLILITY")]
        CounerKing = 1 << 21,

        [XmlEnum("EFFECT_MELEE")]
        Melee = 1 << 22,

        [XmlEnum("EFFECT_RANGED")]
        Ranged = 1 << 23,

        [XmlEnum("EFFECT_SIEGE")]
        Siege = 1 << 24,

        [XmlEnum("EFFECT_UNSUMMON_DUMMY")]
        UnsummonDummy = 1 << 25,

        [XmlEnum("EFFECT_HORN")]
        Horn = 1 << 26,

        [XmlEnum("EFFECT_DRAW")]
        Draw = 1 << 27,

        [XmlEnum("EFFECT_SCORCH")]
        Scorch = 1 << 28,

        [XmlEnum("EFFECT_CLEAR_SKY")]
        ClearSky = 1 << 29,

        [XmlEnum("EFFECT_SUMMON_CLONES")]
        SummonClones = 1 << 30,

        [XmlEnum("EFFECT_IMPROVE_NEIGHBOURS")]
        ImproveNeighbours = 1 << 31,

        [XmlEnum("EFFECT_NURSE")]
        Nurse = 1 << 32,

        [XmlEnum("EFFECT_DRAW_X2")]
        Draw2 = 1 << 33,

        [XmlEnum("EFFECT_SAME_TYPE_MORALE")]
        SameTypeMorale = 1 << 34
    }
}